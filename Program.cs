using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MASBot.Services;

namespace MASBot
{
	public class Program
	{
		private readonly IConfiguration _config;
		private DiscordSocketClient _client;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public Program()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile(path: "config.json");

			_config = builder.Build();
		}

		public async Task MainAsync()
		{
			await using var services = ConfigureServices();

			var client = services.GetRequiredService<DiscordSocketClient>();
			_client = client;

			_client.Log += LogAsync;
			_client.Ready += ReadyAsync;
			services.GetRequiredService<CommandService>().Log += LogAsync;

			await client.LoginAsync(TokenType.Bot, _config["Token"]);
			await client.StartAsync();

			await services.GetRequiredService<CommandHandler>().InitializeAsync();

			await Task.Delay(-1);
		}

		private Task ReadyAsync()
		{
			Console.WriteLine($"Connected as -> [{_client.CurrentUser}] :)");
			return Task.CompletedTask;
		}

		private static Task LogAsync(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private ServiceProvider ConfigureServices()
		{
			return new ServiceCollection()
				.AddSingleton(_config)
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton<CommandService>()
				.AddSingleton<CommandHandler>()
				.BuildServiceProvider();
		}
	}
}