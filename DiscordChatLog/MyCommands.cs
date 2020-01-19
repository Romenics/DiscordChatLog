using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;


namespace DiscordChatLog {
	public class MyCommands {

		public static List <Character> Characters = new List<Character>();

		public static List <string> Sacraments = new List<string> ();

		public class Character {

			public string Name;

			public int[] Stats = new int[8];
			public int Endurance;
			public int Agility;
			public int Sense;
			public int Intelligence;
			public int Will;
			public int Charm;
			public int Luck;
		}


		[Command ("ping")]
		public async Task Ping (CommandContext context) {

			string message = context.Message.Content;

			IReadOnlyList<DiscordChannel> AllChannels = context.Guild.Channels;
			for (int i = 0; i < AllChannels.Count; i++) {

				DSharpPlus.Entities.DiscordChannel channel = await Program.discord.GetChannelAsync(AllChannels[i].Id);

				if (channel.Type == ChannelType.Text){
					DiscordMessage mess = await  Program.discord.SendMessageAsync(channel, message);
					await mess.DeleteAsync();
				}
			}

			await context.Message.DeleteAsync();

		}

		[Command ("invite")]
		public async Task Invite (CommandContext context) {
			await context.RespondAsync ("https://discordapp.com/oauth2/authorize?client_id=642738745526714389&scope=bot&permissions=8");
		}

		[Command ("Save")]
		public async Task Save (CommandContext context) {

			await context.RespondAsync ("Request to save all message from " + context.Channel.Name);

			// Записываем данные самого нового сообщения
			string ChatLog = context.Message.Content;
			ulong LastMessage = context.Message.Id;

			// Счетчик сообщений и итераций
			int Iterations = 0;
			ulong TotalMessages = 0;

			// Пока есть сообщения, эта переменная false
			bool AllSaved = false;

			// Проверяем наличие папки
			if (Directory.Exists ("./Logs") == false) {
				Directory.CreateDirectory ("./Logs");
			}

			string time = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
			Console.WriteLine (time);
			//Создаем файл с названием канала
			StreamWriter streamWriter = new StreamWriter ("./Logs/" + context.Channel.Name + "-" + time + ".csv", true, System.Text.Encoding.Unicode);

			while (AllSaved != true) {
				// Начало новой итерации
				Iterations ++;

				// Получаем новые 100 сообщений вниз, от последнего
				IReadOnlyList<DiscordMessage> AllMessage = await context.Channel.GetMessagesAsync (100, LastMessage);

				// Сохраняем все сообщения
				for (int i = 0; i < AllMessage.Count; i++) {
					ChatLog += AllMessage[i].CreationTimestamp.DateTime.ToString () + ";" + AllMessage[i].Id + ";" + AllMessage[i].Author.Username + ";" + AllMessage[i].Content + "\n";
					TotalMessages ++;
				}
				// Записываем ID последнего сообщения, чтобы начать отсчет с него в следующей итерации
				LastMessage = AllMessage[AllMessage.Count - 1].Id;

				// Если сообщений меньше 100, то значит эта итерации последняя
				if (AllMessage.Count < 100) {
					AllSaved = true;
				}
				streamWriter.WriteLine (ChatLog);
				ChatLog = string.Empty;

				// Итоги итерации
				Console.WriteLine ("Iterations: " + Iterations + ", Total messages: " + TotalMessages);
			}

			streamWriter.Close();
		}
	}
}
