using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwitchBot.Runner;
using TwitchBot.Service.Implementation;
using TwitchBot.Service.Interfaces;
using TwitchBot.Utility;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
	services.AddHttpClient("TwitchAuthClient");
	services.AddScoped<TokenHandler>();
	services.AddHttpClient("TwitchBotClient").AddHttpMessageHandler<TokenHandler>();

	services.AddScoped<TwitchAuthService>();
	services.AddScoped<TwitchAPIClient>();
	services.AddSingleton<TwitchUserSession>();

	services.AddScoped<StartBrowser>();

	services.AddScoped<IBotService, TwitchBotService>();
	services.AddHostedService<BotRunner>();
});

builder.ConfigureLogging(logging =>
{
	logging.ClearProviders();
	logging.AddConsole();
});

var host = builder.Build();
await host.RunAsync();