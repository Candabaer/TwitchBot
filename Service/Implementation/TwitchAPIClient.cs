using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitchBot.Model;
using TwitchBot.Utility;

namespace TwitchBot.Service.Implementation
{
	public class TwitchAPIClient
	{
		HttpClient _client;
		private TwitchUserSession _accessTokenService;
		private IConfiguration _config;
		public TwitchAPIClient(IHttpClientFactory httpClientFactory, TwitchUserSession accessTokenService,
			IConfiguration config)
		{
			_config = config;
			_client = httpClientFactory.CreateClient("TwitchBotClient");
			_accessTokenService = accessTokenService;
		}


		public async Task SendMessage()
		{

			var body = new
			{
				broadcaster_id = _accessTokenService._identity.user_id,
				sender_id = _accessTokenService._identity.user_id,
				message = "Hello from my bot"
			};

			var json = JsonSerializer.Serialize(body);

			var content = new StringContent(
				json,
				Encoding.UTF8,
				"application/json"
			);


			await _client.PostAsync("https://api.twitch.tv/helix/chat/messages", content); 
		}
	}
}