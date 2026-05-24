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
		public string chatter_user_id { get; set; }
		public Message message { get; set; }
		public string text => message.text;

	}

	public class Message
	{
		public string text { get; set; }

	}
}