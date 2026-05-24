using System.Threading.Channels;
using TwitchBot.Model;
using TwitchBot.Model.WebSocket;
using TwitchBot.Service.Interfaces;

namespace TwitchBot.Service.Implementation
{
	public class TwitchBotService : IBotService
	{
		TwitchWebSocketClient _webSocketClient;
		TwitchAPIClient _client;
		ChatReader _textParser;
		private readonly ChannelReader<ReceiverEvent> _reader;
		public TwitchBotService(IHttpClientFactory httpClientFactory, TwitchAPIClient twitchAPIClient,
			TwitchWebSocketClient twitchWebSocketClient, ChatReader parser)
		{
			_textParser = parser;
			_client = twitchAPIClient;
			_webSocketClient = twitchWebSocketClient;
			_reader = twitchWebSocketClient.Messages;
		}

		public async Task Run(CancellationToken stop)
		{
			await _webSocketClient.Connect();

			var receive = _webSocketClient.ReceiveLoop(stop, _client.SubscribeToChat);
			var consume = Consume(stop);

			await Task.WhenAll(receive, consume);
		}

		private async Task Consume(CancellationToken stop)
		{
			await foreach (var msg in _reader.ReadAllAsync(stop))
				await _textParser.Parse(msg);
		}
	}
}
