using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;

namespace DiscordChatLog {

	public class Program {
		
		public static DiscordClient discord;

		static CommandsNextModule commands;

		public static int PartyCount;
		public static string LastDeletedMessage;

		
		static void Main (string[] args) {
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		static async Task MainAsync (string[] args) {
			
			//Настройка базовой конфигурации бота
			DiscordConfiguration DiscordConfig = new DiscordConfiguration {
				Token = "NjQyNzM4NzQ1NTI2NzE0Mzg5.XcbY6A.koIuF0qRBDga6sMD95vNvPMUTuI",
				TokenType = TokenType.Bot,
				UseInternalLogHandler = true, 
				LogLevel = LogLevel.Debug
			};
			
			discord = new DiscordClient (DiscordConfig);

			//Настройка списка комманд
			CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration {
				StringPrefix = "!!",
				EnableMentionPrefix = true,
				EnableDms = false
			};

			commands = discord.UseCommandsNext (commandsConfig);
			commands.RegisterCommands <MyCommands> ();
			Console.WriteLine ("Bot staterted 2.0");
			

			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}
