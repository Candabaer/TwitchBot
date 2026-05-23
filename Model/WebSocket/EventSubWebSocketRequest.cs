using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model.WebSocket
{
	public class EventSubWebSocketRequest
	{
		public EventSubWebSocketRequest(string type, string version, Condition condition, Transport transport)
		{
			this.type = type;
			this.version = version;
			this.condition = condition;
			this.transport = transport;
		}

		public string type { get; set; }
		public string version { get; set; }
		public Condition condition { get; set; }
		public Transport transport { get; set; }
	}

	public class Condition
	{
		public Condition(IdentityToken identityToken)
		{
			this.broadcaster_user_id = identityToken.user_id;
			this.user_id = identityToken.user_id;
		}
		public string broadcaster_user_id { get; set; }
		public string user_id { get; set; }
	}

	public class Transport
	{
		public Transport(string method, string session_id)
		{
			this.method = method;
			this.session_id = session_id;
		}

		public string method { get; set; }
		public string session_id { get; set; }
	}

}
