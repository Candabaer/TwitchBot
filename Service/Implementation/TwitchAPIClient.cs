using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitchBot.Model;
using TwitchBot.Model.WebSocket;
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


		public async Task SendMessage(string message)
		{
			var body = new
			{
				broadcaster_id = _accessTokenService._identity.user_id,
				sender_id = _accessTokenService._identity.user_id,
				message = message
			};

			var json = JsonSerializer.Serialize(body);

			var content = new StringContent(
				json,
				Encoding.UTF8,
				"application/json"
			);


			await _client.PostAsync("https://api.twitch.tv/helix/chat/messages", content);
		}

		internal async Task SubscribeToChat(string webSocketSessionId)
		{
			var condition = new Condition(_accessTokenService._identity);
			var transport = new Transport("websocket", webSocketSessionId);

			var eventSubscriptionChat = new EventSubWebSocketRequest("channel.chat.message", "1", condition, transport);
			var json = JsonSerializer.Serialize(eventSubscriptionChat);

			var content = new StringContent(
				json,
				Encoding.UTF8,
				"application/json"
			);

			var result = await _client.PostAsync("https://api.twitch.tv/helix/eventsub/subscriptions", content);
			var body = await result.Content.ReadAsStringAsync();
			Console.WriteLine(body);
		}
	}
}