﻿using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	public class User : EntityData
	{
		public string DeviceIdent { get; set; }

		public string SnapchatUsername { get; set; }

		public string SnapchatAuthToken { get; set; }

		public Boolean AuthExpired { get; set; }

		public Boolean NewUser { get; set; }
	}
}