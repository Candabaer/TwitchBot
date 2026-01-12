using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model
{
	public class DeviceCodeResponse
	{
		public string device_code { get; set; } = string.Empty;
		public string user_code { get; set; } = string.Empty;
		public string verification_uri { get; set; } = string.Empty;
		public int interval { get; set; }
	}
}
