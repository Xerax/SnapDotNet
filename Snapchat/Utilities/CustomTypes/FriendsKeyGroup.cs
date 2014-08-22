﻿using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;

namespace SnapDotNet.Utilities.CustomTypes
{
	public class FriendsKeyGroup
		: List<Friend>, IAlphaKeyGroup<Friend>
	{
		public FriendsKeyGroup() { }

		public FriendsKeyGroup(string key, string friendlyKey, Color foregroundColour, Color backgroundColour)
		{
			Key = key;
			FriendlyKey = friendlyKey;
			ForegroundColour = foregroundColour;
			BackgroundColour = backgroundColour;
		}

		public Color BackgroundColour { get; private set; }

		public Color ForegroundColour { get; private set; }

		public string Key { get; private set; }

		public string FriendlyKey { get; private set; }

		private static List<FriendsKeyGroup> CreateGroups()
		{
			const string keys = "abcdefghijklmnopqrstuvwxyz#";

			var list = keys.Select(
				key =>
					new FriendsKeyGroup(key.ToString(), key.ToString().ToUpperInvariant(),
						Color.FromArgb(0xFF, 0x9B, 0x55, 0xA0),
						Color.FromArgb(0x00, 0x00, 0x00, 0x00))).ToList();

			list.Add(new FriendsKeyGroup("!",
				"BLOCKED",
				Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF),
				Color.FromArgb(0xFF, 0xE9, 0x27, 0x54)));

			return list;
		}

		public static ObservableCollection<FriendsKeyGroup> CreateGroups(ObservableCollection<Friend> items)
		{
			var list = CreateGroups();

			Debug.WriteLine("[FriendsKeyGroup] Starting to Create Groups, based off of {0} friends", items.Count);

			foreach (var friend in items)
			{
				if (friend.FriendRequestState == FriendRequestState.Blocked)
				{
					list.Find(a => a.Key == "!").Add(friend);
					Debug.WriteLine("[FriendsKeyGroup] Added friend {0} to Blocked", friend.FriendlyName);
				}
				else
				{
					var key = friend.FriendlyName.ToUpperInvariant()[0];
					if (char.IsLetter(key))
					{
						list.Find(a => a.Key == key.ToString().ToLowerInvariant()).Add(friend);
						Debug.WriteLine("[FriendsKeyGroup] Added friend {0} with key {1} to {2}", friend.FriendlyName, key, key);
					}
					else
					{
						list.Find(a => a.Key == "#").Add(friend);
						Debug.WriteLine("[FriendsKeyGroup] Added friend {0} with key {1} to {2}", friend.FriendlyName, key, "numbers");
					}
				}
			}

			//foreach (var group in list)
			//	group.OrderByDescending(g => g.FriendlyName[0]);

			return new ObservableCollection<FriendsKeyGroup>(list);
		}
	}
}
