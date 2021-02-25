using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MASBot.Modules
{
	public class GameCommands : ModuleBase
	{
        [Command("8ball")]
        [Alias("ask")]
        public async Task AskEightBall([Remainder] string args = null)
        {
	        var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            var replies = new List<string> 
            {
	            "yes", 
	            "no", 
	            "maybe", 
	            "hazy..."
            };

            embed.WithColor(new Color(0, 255, 0));
            embed.Title = "Welcome to the 8-ball!";

            sb.AppendLine($"{Context.User.Username},");
            sb.AppendLine();

            if (args == null)
            {
	            sb.AppendLine("Sorry, I can't answer a question you didn't ask!");
            }
            else
            {
	            var answer = replies[new Random().Next(replies.Count - 1)];

                sb.AppendLine($"You asked: [**{args}**]...");
                sb.AppendLine();
                sb.AppendLine($"...your answer is [**{answer}**]");

                switch (answer)
                {
                    case "yes":
                        {
                            embed.WithColor(new Color(0, 255, 0));
                            break;
                        }
                    case "no":
                        {
                            embed.WithColor(new Color(255, 0, 0));
                            break;
                        }
                    case "maybe":
                        {
                            embed.WithColor(new Color(255, 255, 0));
                            break;
                        }
                    case "hazy...":
                        {
                            embed.WithColor(new Color(255, 0, 255));
                            break;
                        }
                }
            }

            embed.Description = sb.ToString();

            await ReplyAsync(null, false, embed.Build());
        }
    }
}
