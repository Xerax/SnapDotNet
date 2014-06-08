﻿using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using SnapDotNet.Core.Snapchat.Models.New;

namespace Snapchat.Converters.Conversations
{
	public class SnapColourConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var conversation = (ConversationResponse) value;
			if (conversation == null)
				return null;

			// get the snap object
			if (conversation.LastSnap == null)
				return null;

			return conversation.LastSnap.IsImage
				? new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0x27, 0x54))
				: new SolidColorBrush(Color.FromArgb(0xFF, 0x9B, 0x55, 0xA0));
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}