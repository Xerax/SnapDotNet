﻿using System;
using System.Collections.Generic;
using SnapDotNet.Core.Snapchat.Models.New;

namespace SnapDotNet.Core.Snapchat.Models.AppSpecific.RawNotificationTransfer
{
	public class RawNotificationTransferContainer
	{
		public RawNotificationTransferContainer()
		{
			SnapNotifications = new List<SnapNotification>();
			ScreenshotNotifications = new List<ScreenshotNotification>();
			FriendAddedNotifications = new List<FriendAddedNotification>();
			ChatNotifications = new List<ChatNotification>();
		}

		public String SuperSecret = "careful guys, http://i.imgur.com/ye5udHZ.gif";

		public List<SnapNotification> SnapNotifications { get; set; }

		public List<ScreenshotNotification> ScreenshotNotifications { get; set; } 

		public List<FriendAddedNotification> FriendAddedNotifications { get; set; }

		public List<ChatNotification> ChatNotifications { get; set; }
	}

	public class SnapNotification
	{
		public String Id { get; set; }

		public String Sender { get; set; }
	}

	public class ScreenshotNotification
	{
		public String Id { get; set; }

		public String Sender { get; set; }
	}

	public class FriendAddedNotification
	{
		public String Sender { get; set; }
	}

	public class ChatNotification
	{
		public String Id { get; set; }

		public String Sender { get; set; }

		public MessageBodyType Type { get; set; }
	}
}