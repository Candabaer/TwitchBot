using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Utility
{
	public class StartBrowser
	{
		public void RunDefaultBrowser(string targetURI)
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = targetURI,
				UseShellExecute = true
			});
		}
	}
}
