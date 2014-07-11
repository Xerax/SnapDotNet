﻿using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Snapchat.SnapLogic.Converters.Json;
using SnapDotNet.Core.Miscellaneous.CustomTypes;
using SnapDotNet.Core.Miscellaneous.Models;

namespace Snapchat.SnapLogic.Models.New
{
	[DataContract]
	public class ConversationResponse
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "conversation_messages")]
		public ConversationMessages ConversationMessages { get; set; }

		[DataMember(Name = "conversation_state")]
		public ConversationState ConversationState { get; set; }

		[DataMember(Name = "last_chat_actions")]
		public LastChatActions LastChatActions { get; set; }

		[DataMember(Name = "id")]
		public String Id { get; set; }

		[DataMember(Name = "iter_token")]
		public String IterToken { get; set; }

		[DataMember(Name = "last_interaction_ts")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastInteraction { get; set; }

		[DataMember(Name = "last_snap")]
		public Snap LastSnap { get; set; }

		[DataMember(Name = "participants")]
		public ObservableCollection<String> Participants { get; set; }

		[DataMember(Name = "pending_chats_for")]
		public ObservableCollection<String> PendingChatsFor { get; set; }

		[DataMember(Name = "pending_received_snaps")]
		public ObservableCollection<Snap> PendingReceivedSnaps { get; set; }
	}

	[DataContract]
	public class ConversationMessages
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "messages")]
		public ObservableCollection<MessageContainer> Messages { get; set; }
		
		[DataMember(Name = "messaging_auth")]
		public MessagingAuthentication MessagingAuthentication { get; set; }
	}

	[DataContract]
	public class MessageContainer
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "snap")]
		public Snap Snap { get; set; }

		[DataMember(Name = "chat_message")]
		public ChatMessage ChatMessage { get; set; }

		[DataMember(Name = "iter_token")]
		public String IterToken { get; set; }
	}

	[DataContract]
	public class MessagingAuthentication
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "mac")]
		public String Mac { get; set; }

		[DataMember(Name = "payload")]
		public String Payload { get; set; }
	}

	[DataContract]
	public class ConversationState
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "user_chat_releases")]
		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserChatReleases { get; set; }

		[DataMember(Name = "user_sequences")]
		public ObservableDictionary<string, int> UserSequences { get; set; }

		[DataMember(Name = "user_snap_releases")]
		public ObservableDictionary<string, ObservableDictionary<string, Int64>> UserSnapReleases { get; set; }
	}

	[DataContract]
	public class LastChatActions
		: NotifyPropertyChangedBase
	{
		[DataMember(Name = "last_read_timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime LastRead { get; set; }

		[DataMember(Name = "last_reader")]
		public String LastReader { get; set; }

		[DataMember(Name = "last_write")]
		[JsonConverter(typeof (UnixDateTimeConverter))]
		public DateTime LastWrite { get; set; }

		[DataMember(Name = "last_write_type")]
		public String LastWriteType { get; set; }

		[DataMember(Name = "last_writer")]
		public String LastWriter { get; set; }
	}
}
