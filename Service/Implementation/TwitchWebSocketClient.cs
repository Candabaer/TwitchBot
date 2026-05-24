using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
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
		private readonly Channel<ReceiverEvent> _channel;
		public ChannelReader<ReceiverEvent> Messages => _channel.Reader;

		public string WebSocketSessionId { get; set; }

		public TwitchWebSocketClient()
		{
			_channel = Channel.CreateBounded<ReceiverEvent>(
				new BoundedChannelOptions(1000)
				{
					SingleWriter = true,
					SingleReader = false,
					FullMode = BoundedChannelFullMode.Wait
				});
		}

		public async Task Connect()
		{
			_cts = new CancellationTokenSource();
			Console.WriteLine("Connecting to websocket...");
			await _ClientWebSocket.ConnectAsync(new Uri("wss://eventsub.wss.twitch.tv/ws"), _cts.Token);
			Console.WriteLine("Connected. State: " + _ClientWebSocket.State);
		}

		public async Task ReceiveLoop(CancellationToken token, Func<string, Task> Subscribe)
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
				Console.WriteLine(message);
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
				await _channel.Writer.WriteAsync(model.payload.ReceiverEvent);
			}
		}
		public bool IsOpen() => _ClientWebSocket.State == WebSocketState.Open;

	}
}
