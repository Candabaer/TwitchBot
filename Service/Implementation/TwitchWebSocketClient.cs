using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchBot.Model.WebSocket;

namespace TwitchBot.Service.Implementation
{
	public class TwitchWebSocketClient
	{
		private ClientWebSocket _ClientWebSocket = new ClientWebSocket();
		private CancellationTokenSource _cts;
		private Task? _receiveTask;

		public async Task Connect()
		{
			_cts = new CancellationTokenSource();
			Console.WriteLine("Connecting to websocket...");
			await _ClientWebSocket.ConnectAsync(new Uri("wss://eventsub.wss.twitch.tv/ws"), _cts.Token);
			Console.WriteLine("Connected. State: " + _ClientWebSocket.State);

			try
			{
				_receiveTask = Task.Run(() => ReceiveLoop(_cts.Token));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public async Task ReceiveLoop(CancellationToken token)
		{
			var buffer = new byte[8192];
			Console.WriteLine("Listening now to the Websocket");
			while (_ClientWebSocket.State == WebSocketState.Open && !token.IsCancellationRequested)
			{
				var result = await _ClientWebSocket.ReceiveAsync(
					buffer,
					token
				);

				if (result.MessageType == WebSocketMessageType.Close)
				{
					await _ClientWebSocket.CloseAsync(
						WebSocketCloseStatus.NormalClosure,
						"Closing",
						token
					);
					break;
				}
				Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
				var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
				var model = JsonSerializer.Deserialize<WebSocketReceive>(message);
				//Console.WriteLine(message);
			}
		}

		public bool IsOpen() => _ClientWebSocket.State == WebSocketState.Open;

	}
}
