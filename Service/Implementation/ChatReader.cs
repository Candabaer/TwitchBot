using System.Runtime.CompilerServices;
using TwitchBot.Model.WebSocket;

namespace TwitchBot.Service.Implementation
{
	public class ChatReader
	{
		private const string UNKNOWN = "unknown";
		private TwitchAPIClient _apiClient;
		private readonly BettingGame _bettingGame;
		private readonly UserService _userService;

		public ChatReader(TwitchAPIClient twitchApiClient, BettingGame bettingGame, UserService userService)
		{
			_apiClient = twitchApiClient;
			_bettingGame = bettingGame;
			_userService = userService;
		}
		public async Task Parse(ReceiverEvent message)
		{
			var result = ChatCommand(message);
			_userService.CreateUser(message);

			if (result == UNKNOWN)
				return;

			await _apiClient.SendMessage(result);
		}

		private void CreateNewUser(ReceiverEvent message)
		{

		}

		private string ChatCommand(ReceiverEvent message)
		{
			return message.text switch
			{
				string t when t.StartsWith("!help") => HandleHelp(),
				string t when t.StartsWith("!ping") => "pong",
				string t when t.StartsWith("!bet") => TakeBet(message),
				string t when t.StartsWith("!Axodor") => "Is'n hengst!",
				string t when t.StartsWith("!Score") => GetScore(message),
				_ => UNKNOWN
			};
		}

		private string TakeBet(ReceiverEvent message)
		{
			_bettingGame.CreateBet(message);
			return UNKNOWN;
		}

		private string HandleHelp()
		{
			return "GoLoveYourself";
		}

		private string GetScore(ReceiverEvent message)
		{


			return $"Your Score currently is {message.text}";
		}
	}
}