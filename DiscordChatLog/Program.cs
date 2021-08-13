using System;
using System.IO;
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
		//File loading from Token.txt
		public static string DiscordToken;

		
		static void Main (string[] args) {
			DiscordToken = ReadTxt ("Token.txt");
			Console.WriteLine ("You Token: " + DiscordToken);
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		static async Task MainAsync (string[] args) {
			
			// Base configuration bot
			DiscordConfiguration DiscordConfig = new DiscordConfiguration {
				Token = DiscordToken,
				TokenType = TokenType.Bot,
				UseInternalLogHandler = true, 
				LogLevel = LogLevel.Debug
			};
			
			discord = new DiscordClient (DiscordConfig);

			// Setting up command list
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


		/// <summary>
		/// Load each line from txt file wich located in project root
		/// File name example: Test (without extantion)
		/// </summary>
		public static string[] ReadTxtLines (string FileName) {

			string Path = @"./" + FileName + ".txt";

			if (File.Exists (Path) == false) {
				return new string[0];
			}
			else {
				return File.ReadAllLines (Path);
			}
		}


		/// <summary>
		/// Load all lines in one string from txt file wich located in project root
		/// File name example: Test (without extantion)
		/// </summary>
		public static string ReadTxt (string FileName) {

			string Path = @"./" + FileName;

			if (File.Exists (Path) == false) {
				return string.Empty;
			}
			else {
				return File.ReadAllText (Path);
			}
		}
	}
}
