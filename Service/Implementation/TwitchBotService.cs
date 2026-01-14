using TwitchBot.Model;
using TwitchBot.Service.Interfaces;

namespace TwitchBot.Service.Implementation
{
	public class TwitchBotService : IBotService
	{
		TwitchAPIClient _client;
		public TwitchBotService(IHttpClientFactory httpClientFactory, TwitchAPIClient twitchAPIClient)
		{
			_client = twitchAPIClient;
		}



		public async Task Run()
		{
			await _client.SendMessage();
		}

		public void ConnectToProvider()
		{

		}
	}
}
