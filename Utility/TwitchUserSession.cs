using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitchBot.Model;
using TwitchBot.Model.WebSocket;
using TwitchBot.Service.Implementation;

namespace TwitchBot.Utility
{
	public class TwitchUserSession
	{
		public AccessToken _userAccessToken { get; private set; }
		public IdentityToken _identity { get; private set; }
		public DateTime _expireTime { get; private set; }

		private TwitchAuthService _twitchAuthService;

		public TwitchUserSession(TwitchAuthService twitchAuthService) { 
			_twitchAuthService = twitchAuthService;
		}

		public async Task SetupAuth()
		{
			var deviceCode = await _twitchAuthService.RequestDeviceCodeAsync();
			_userAccessToken = await _twitchAuthService.PollForAccessToken(deviceCode);
			_expireTime = DateTime.Now.AddSeconds(_userAccessToken.expires_in - 60);

			_identity = await _twitchAuthService.GetIdentityTokenAsync(_userAccessToken);
		}

		public async Task ForceRefresh()
		{
			_userAccessToken = await _twitchAuthService.RefreshAccessToken(_userAccessToken);
		}
	}
}
