using System.Text.Json.Serialization;

namespace TwitchBot.Model.WebSocket
{
	public class Subscription
	{
		public string id { get; set; }
	}
	public class ReceiverEvent
	{
		public string chatter_user_name { get; set; }
		public Message message { get; set; }
	}

	public class Message
	{
		public string text { get; set; }

	}
}