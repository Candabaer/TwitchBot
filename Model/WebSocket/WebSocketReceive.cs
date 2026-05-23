using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model.WebSocket
{
	internal class WebSocketReceive
	{
		public MetaData metadata { get; set; }
		public Payload payload { get; set; }
	}
}
