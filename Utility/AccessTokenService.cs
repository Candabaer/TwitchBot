using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.Model;
using TwitchBot.Service.Implementation;

namespace TwitchBot.Utility
{
	public class AccessTokenService
	{
		public AccessToken _accessToken { get; private set; }
		public DateTime _expireTime { get; private set; }

		TwitchAuthService _twitchAuthService;
		public AccessTokenService(TwitchAuthService twitchAuthService) { 
			_twitchAuthService = twitchAuthService;
		}

		public async Task SetupAuth()
		{
			var deviceCode = await _twitchAuthService.RequestDeviceCodeAsync();
			_accessToken = await _twitchAuthService.PollForAccessToken(deviceCode);
			_expireTime = DateTime.Now.AddSeconds(_accessToken.expires_in - 60);
		}

		public async Task ForceRefresh()
		{
			_accessToken = await _twitchAuthService.RefreshAccessToken(_accessToken);
		}
	}
}
