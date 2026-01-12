using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model
{
	public class AccessToken
	{
		public int expires_in { get; set; }
		public string[] scope { get; set; } = new string[0];
		public string access_token { get; set; } = string.Empty;
		public string refresh_token { get; set; } = string.Empty;
		public string token_type { get; set; } = string.Empty;
	}
}
