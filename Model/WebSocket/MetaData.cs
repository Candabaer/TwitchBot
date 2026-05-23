using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model.WebSocket
{
	public class MetaData
	{

		public string message_id {  get; set; }
		public string message_type { get; set; }
		public string message_timestamp { get; set; }
	}
}
