﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapDotNet.Azure.MobileService.DataObjects
{
	[Table("snapdotnet.SnapchatChats")]
	public class SnapchatChat : BaseEntity
	{
		public String UserId { get; set; }

		public String ChatId { get; set; }

		public String SenderName { get; set; }
	}
}
