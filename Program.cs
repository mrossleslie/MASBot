using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace MASBot
{
	public class Program
	{
		private readonly DiscordSocketClient _client;
		private readonly IConfiguration _config;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public Program()
		{
			_client = new DiscordSocketClient();
			_client.Ready += ReadyAsync;
			_client.Log += Log;
			_client.MessageReceived += MessageReceivedAsync;

			var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile(path: "config.json");
			_config = builder.Build();
		}

		public async Task MainAsync()
		{
			await _client.LoginAsync(TokenType.Bot, _config["Token"]);
			await _client.StartAsync();

			await Task.Delay(-1);
		}
		private Task ReadyAsync()
		{
			Console.WriteLine($"Connected as -> [{_client.CurrentUser}] :)");
			return Task.CompletedTask;
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private async Task MessageReceivedAsync(SocketMessage message)
		{
			if (message.Author.Id == _client.CurrentUser.Id)
				return;

			if (message.Content ==  $"{_config["Prefix"]}hello")
			{
				await message.Channel.SendMessageAsync("world!");
			}
		}
	}
}