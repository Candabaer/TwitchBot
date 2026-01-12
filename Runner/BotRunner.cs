using Microsoft.Extensions.Hosting;
using TwitchBot.Service.Implementation;
using TwitchBot.Service.Interfaces;
using TwitchBot.Utility;

namespace TwitchBot.Runner
{

	public class BotRunner : BackgroundService
	{
		private readonly IBotService _bot;
		AccessTokenService _accessTokenService;

		public BotRunner(IBotService bot, AccessTokenService accessTokenService)
		{
			_accessTokenService = accessTokenService;
			_bot = bot;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				await _accessTokenService.SetupAuth();
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine(e.Message);
			}

			_bot.Run();
		}
	}
}
