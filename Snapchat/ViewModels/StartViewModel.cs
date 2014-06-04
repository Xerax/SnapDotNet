﻿using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Snapchat.Common;
using Snapchat.Pages;
using SnapDotNet.Core.Miscellaneous.Models.Atlas;
using SnapDotNet.Core.Snapchat.Api.Exceptions;

namespace Snapchat.ViewModels
{
	public sealed class StartViewModel
		: ObservableObject
	{
		public StartViewModel()
		{
			LogInCommand = new RelayCommand(LogInAsync);
		}

		/// <summary>
		/// Gets or sets the current username entered by the user.
		/// </summary>
		public string CurrentUsername
		{
			get { return _currentUsername; }
			set { TryChangeValue(ref _currentUsername, value); }
		}
		private string _currentUsername;

		/// <summary>
		/// Gets or sets the current password entered by the user.
		/// </summary>
		public string CurrentPassword
		{
			get { return _currentPassword; }
			set { TryChangeValue(ref _currentPassword, value); }
		}
		private string _currentPassword;

		/// <summary>
		/// Gets the command to log into Snapchat.
		/// </summary>
		public ICommand LogInCommand
		{
			get { return _logInCommand; }
			private set { TryChangeValue(ref _logInCommand, value); }
		}
		private ICommand _logInCommand;

		/// <summary>
		/// Attempts to log into Snapchat using the username and password stored in <see cref="CurrentUsername"/>
		/// and <see cref="CurrentPassword"/> respectively.
		/// </summary>
		private async void LogInAsync()
		{
#if DEBUG
			if (string.IsNullOrEmpty(CurrentUsername.Trim()))
			{
				// alex is hacky, and wants to use his other auth token
				App.SnapchatManager.UpdateAuthToken(CurrentPassword);
				await App.SnapchatManager.Endpoints.GetUpdatesAsync();
				return;
			}
#endif

			// Username or password cannot be null.
			if (string.IsNullOrEmpty(CurrentUsername.Trim()) || string.IsNullOrEmpty(CurrentPassword.Trim()))
			{
				await (new MessageDialog(App.Strings.GetString("InvalidCredentialsExceptionFriendlyMessage"))).ShowAsync();
				return;
			}

			// Show progress indicator.
			var progress = StatusBar.GetForCurrentView().ProgressIndicator;
			progress.Text = App.Strings.GetString("StatusLoggingIn");
			await progress.ShowAsync();

			try
			{
				// Try to log into Snapchat.
				await App.SnapchatManager.Endpoints.AuthenticateAsync(CurrentUsername, CurrentPassword);

				// Register this device for push notifications.
				await
					App.MobileService.GetTable<User>()
						.InsertAsync(new User
						{
							AuthExpired = false,
							NewUser = true,
							DeviceIdent = App.DeviceId,
							SnapchatAuthToken = App.SnapchatManager.AuthToken,
							SnapchatUsername = App.SnapchatManager.Username
						});

				// Navigate to the main page.
				var frame = Window.Current.Content as Frame;
				if (frame != null) frame.Navigate(typeof(MainPage));
			}
			catch (InvalidCredentialsException)
			{
				(new MessageDialog(App.Strings.GetString("InvalidCredentialsExceptionFriendlyMessage"))).ShowAsync();
			}
			catch (InvalidHttpResponseException e)
			{
				(new MessageDialog(App.Strings.GetString("InvalidHttpResponseExceptionMessage"))).ShowAsync();
				Debug.WriteLine(e.Message);
			}
			finally
			{
				// Hide progress indicator.
				progress.Text = String.Empty;
				progress.HideAsync();

				// Clear the password field.
				CurrentPassword = null;
			}
		}
	}
}
