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
		//Загружается из файла Token.txt
		public static string DiscordToken;

		
		static void Main (string[] args) {
			DiscordToken = ReadTxt ("Token.txt");
			Console.WriteLine ("You Token: " + DiscordToken);
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		static async Task MainAsync (string[] args) {
			
			//Настройка базовой конфигурации бота
			DiscordConfiguration DiscordConfig = new DiscordConfiguration {
				Token = DiscordToken,
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

		/// <summary>
		/// Выдает содержимое каждой строки txt файла который лежит в корне Unity проекта (рядом с Assets)
		/// Имя файла например Test (без расширения)
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


		/// Выдает содержимое txt файла который лежит в корне Unity проекта (рядом с Assets)
		/// Имя файла например Test.txt
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
