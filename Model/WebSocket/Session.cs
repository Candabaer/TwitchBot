using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model.WebSocket
{
	public class Session
	{
		public string id { get; set; }
		public string status { get; set; }
		public int keepalive_timeout_seconds { get; set; }
		public string reconnect_url { get; set; }
		public string connected_at { get; set; }
	}
}
