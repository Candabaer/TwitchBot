using TwitchBot.Model;
using TwitchBot.Service.Interfaces;

namespace TwitchBot.Service.Implementation
{
	public class TwitchBotService : IBotService
	{
		TwitchWebSocketClient _webSocketClient;
		TwitchAPIClient _client;
		public TwitchBotService(IHttpClientFactory httpClientFactory, TwitchAPIClient twitchAPIClient, TwitchWebSocketClient twitchWebSocketClient)
		{
			_client = twitchAPIClient;
			_webSocketClient = twitchWebSocketClient;
		}



		public async Task Run()
		{
			await _webSocketClient.Connect(_client.SubscribeToChat);
		}



		public void ConnectToProvider()
		{

		}
	}
}
