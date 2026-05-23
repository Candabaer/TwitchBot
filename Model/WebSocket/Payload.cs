using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TwitchBot.Model.WebSocket
{
	public class Payload
	{
		public Session session { get; set; }

		public Subscription subscription { get; set; }

		[JsonPropertyName("event")]
		public ReceiverEvent ReceiverEvent { get; set; }
	}
}
