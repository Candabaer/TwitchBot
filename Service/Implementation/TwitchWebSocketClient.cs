using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TwitchBot.Model.WebSocket;

namespace TwitchBot.Service.Implementation
{
	public class TwitchWebSocketClient
	{
		private ClientWebSocket _ClientWebSocket = new ClientWebSocket();
		private CancellationTokenSource _cts;
		private Task? _receiveTask;

		public string WebSocketSessionId { get; set; }
		private TextParser _textParser;

		public TwitchWebSocketClient(TextParser textParser)
		{
			_textParser = textParser;
		}

		public async Task Connect(Func<string, Task> Subscribe)
		{
			_cts = new CancellationTokenSource();
			Console.WriteLine("Connecting to websocket...");
			await _ClientWebSocket.ConnectAsync(new Uri("wss://eventsub.wss.twitch.tv/ws"), _cts.Token);
			Console.WriteLine("Connected. State: " + _ClientWebSocket.State);

			try
			{
				_receiveTask = Task.Run(() => ReceiveLoop(_cts.Token, Subscribe));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private async Task ReceiveLoop(CancellationToken token, Func<string, Task> Subscribe)
		{
			var buffer = new byte[8192];
			Console.WriteLine("Listening now to the Websocket");
			while (_ClientWebSocket.State == WebSocketState.Open && !token.IsCancellationRequested)
			{
				var result = await _ClientWebSocket.ReceiveAsync(
					buffer,
					token
				);

				var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
				var model = JsonSerializer.Deserialize<WebSocketReceive>(message);

				if (model == null)
					throw new Exception("Evryting in te shitter");

				if (model.metadata.message_type == "session_keepalive")
					continue;

				if (model.metadata.message_type == "session_welcome")
				{
					WebSocketSessionId = model.payload.session.id;
					await Subscribe.Invoke(WebSocketSessionId);
					continue;
				}

				if (result.MessageType == WebSocketMessageType.Close)
				{
					await _ClientWebSocket.CloseAsync(
						WebSocketCloseStatus.NormalClosure,
						"Closing",
						token
					);
					break;
				}
				await _textParser.Parse(model.payload.ReceiverEvent.message.text);
				Console.WriteLine($"{model.payload.ReceiverEvent.chatter_user_name}: {model.payload.ReceiverEvent.message.text}");
			}
		}
		public bool IsOpen() => _ClientWebSocket.State == WebSocketState.Open;

	}
}
