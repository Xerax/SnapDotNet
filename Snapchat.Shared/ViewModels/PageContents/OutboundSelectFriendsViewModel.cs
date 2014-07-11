﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml.Media.Animation;
using Snapchat.CustomTypes;
using Snapchat.Models;
using Snapchat.Pages.PageContents;

namespace Snapchat.ViewModels.PageContents
{
	public class OutboundSelectFriendsViewModel
		: BaseViewModel
	{
		public OutboundSelectFriendsViewModel(OutboundSelectFriends view, byte[] imageData)
		{
			View = view;
			ImageData = imageData;
			SelectedRecipientCollection = new List<string>();

			FriendsCollection =
				SelectFriendskeyGroup<SelectedFriend>.CreateGroups(
					App.SnapchatManager.Account.Friends.Where(f => f.Type != FriendRequestState.Blocked)
						.Select(friend => new SelectedFriend {Friend = friend})
						.ToList(), new CultureInfo("en"));

			StoriesCollection = new ObservableCollection<SelectedOther> { new SelectedOther { OtherName = "My Story", OtherType = OtherType.Story } };

			BestFriendsCollection = new ObservableCollection<SelectedOther>(
				App.SnapchatManager.Account.BestFriends.Take(App.SnapchatManager.Account.NumberOfBestFriends == 0 ? 3 : App.SnapchatManager.Account.NumberOfBestFriends)
					.Select(bestFriend => new SelectedOther { OtherName = bestFriend, OtherType = OtherType.BestFriend })
					.ToList());

			RecentsCollection = new ObservableCollection<SelectedOther>(
				App.SnapchatManager.Account.RecentFriends
					.Select(recent => new SelectedOther { OtherName = recent, OtherType = OtherType.Recent })
					.ToList());
		}

		public OutboundSelectFriends View { get; private set; }

		public ObservableCollection<SelectFriendskeyGroup<SelectedFriend>> FriendsCollection
		{
			get { return _friendsCollection; }
			set { TryChangeValue(ref _friendsCollection, value); }
		}
		private ObservableCollection<SelectFriendskeyGroup<SelectedFriend>> _friendsCollection = new ObservableCollection<SelectFriendskeyGroup<SelectedFriend>>();

		public ObservableCollection<SelectedOther> StoriesCollection
		{
			get { return _storiesCollection; }
			set { TryChangeValue(ref _storiesCollection, value); }
		}
		private ObservableCollection<SelectedOther> _storiesCollection = new ObservableCollection<SelectedOther>();

		public ObservableCollection<SelectedOther> BestFriendsCollection
		{
			get { return _bestFriendsCollection; }
			set { TryChangeValue(ref _bestFriendsCollection, value); }
		}
		private ObservableCollection<SelectedOther> _bestFriendsCollection = new ObservableCollection<SelectedOther>();

		public ObservableCollection<SelectedOther> RecentsCollection
		{
			get { return _recentsCollection; }
			set { TryChangeValue(ref _recentsCollection, value); }
		}
		private ObservableCollection<SelectedOther> _recentsCollection = new ObservableCollection<SelectedOther>();

		public Byte[] ImageData
		{
			get { return _imageData; }
			set { TryChangeValue(ref _imageData, value); }
		}
		private Byte[] _imageData;

		public String SelectedRecipients
		{
			get { return String.Join(", ", SelectedRecipientCollection); }
		}
		public Boolean HasRecipients
		{
			get { return SelectedRecipientCollection.Count > 0; }
		}

		public List<String> SelectedRecipientCollection { get; private set; }

		public void UpdateSelectedRecipients(string username, bool selectedState)
		{
			// do shit
			var friendlyName = username;
			var friend = App.SnapchatManager.Account.Friends.FirstOrDefault(f => f.Name == username);
			if (friend != null)
				friendlyName = friend.FriendlyName;

			if (selectedState)
			{
				// Add it yo
				if (!SelectedRecipientCollection.Contains(friendlyName))
					SelectedRecipientCollection.Insert(0, friendlyName);
			}
			else
			{
				// Remove it yo
				if (SelectedRecipientCollection.Contains(friendlyName))
					SelectedRecipientCollection.Remove(friendlyName);
			}
			
			ExplicitOnNotifyPropertyChanged("SelectedRecipients");
			var state = SelectedRecipientCollection.Any() ? "HasRecipients" : "HasNoRecipients";

			var storyboard = (Storyboard) View.Resources[state];
			if (storyboard == null) return;
			storyboard.Begin();
		}

	}
}
