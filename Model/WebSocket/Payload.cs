using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model.WebSocket
{
	public class Payload
	{
		public Session session { get; set; }
	}
}
