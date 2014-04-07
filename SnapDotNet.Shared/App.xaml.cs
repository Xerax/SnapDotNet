﻿using Microsoft.WindowsAzure.MobileServices;
using SnapDotNet.Apps.Pages;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Networking.PushNotifications;
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
#endif

namespace SnapDotNet.Apps
{
	/// <summary>
	///     Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App
	{
		public static readonly ResourceLoader Loader = new ResourceLoader();

#if WINDOWS_PHONE_APP
		private TransitionCollection _transitions;
#endif

		public static MobileServiceClient MobileService = new MobileServiceClient(
			"https://snapdotnet-push.azure-mobile.net/",
			"FtwlXPgTOpyyNkVazOzoPIzTfwaSUW32"
		);

		/// <summary>
		///     Initializes the singleton application object.  This is the first line of authored code
		///     executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;
		}

		/// <summary>
		/// Gets the current frame.
		/// </summary>
		public static Frame CurrentFrame { get { return (Frame) Window.Current.Content; } }

		/// <summary>
		///     Invoked when the application is launched normally by the end user.  Other entry points
		///     will be used when the application is launched to open a specific file, to display
		///     search results, and so forth.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (Debugger.IsAttached)
			{
				DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			var rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame
				{
					// TODO: change this value to a cache size that is appropriate for your application
					CacheSize = 1
				};

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
#if WINDOWS_PHONE_APP
				// Removes the turnstile navigation for startup.
				if (rootFrame.ContentTransitions != null)
				{
					_transitions = new TransitionCollection();
					foreach (Transition c in rootFrame.ContentTransitions)
					{
						_transitions.Add(c);
					}
				}

				rootFrame.ContentTransitions = null;
				rootFrame.Navigated += RootFrame_FirstNavigated;
#endif

				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				if (!rootFrame.Navigate(typeof (StartPage), e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}
			
			// Register for Push Notifications
			InitNotificationsAsync();

			// Ensure the current window is active
			Window.Current.Activate();
		}

#if WINDOWS_PHONE_APP
		/// <summary>
		///     Restores the content transitions after the app has launched.
		/// </summary>
		/// <param name="sender">The object where the handler is attached.</param>
		/// <param name="e">Details about the navigation event.</param>
		private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
		{
			var rootFrame = sender as Frame;
			rootFrame.ContentTransitions = _transitions ?? new TransitionCollection {new NavigationThemeTransition()};
			rootFrame.Navigated -= RootFrame_FirstNavigated;
		}
#endif

		/// <summary>
		/// 
		/// </summary>
		private static async void InitNotificationsAsync()
		{
#if WINDOWS_PHONE_APP
			// Request a push notification channel.
			var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
			await MobileService.GetPush().RegisterNativeAsync(channel.Uri);
			channel.PushNotificationReceived += (sender, args) =>
			{

			};
#endif
		}
		

		/// <summary>
		///     Invoked when application execution is being suspended.  Application state is saved
		///     without knowing whether the application will be terminated or resumed with the contents
		///     of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private static void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}