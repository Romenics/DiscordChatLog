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

			// Write data of first message
			string ChatLog = context.Message.Content;
			ulong LastMessage = context.Message.Id;

			// Message and iteractions counter
			int Iterations = 0;
			ulong TotalMessages = 0;

			// Until not saved this value false
			bool AllSaved = false;

			// Check folder exist
			if (Directory.Exists ("./Logs") == false) {
				Directory.CreateDirectory ("./Logs");
			}

			string time = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
			Console.WriteLine (time);
			// Create folder with channel name
			StreamWriter streamWriter = new StreamWriter ("./Logs/" + context.Channel.Name + "-" + time + ".csv", true, System.Text.Encoding.Unicode);

			while (AllSaved != true) {
				// Start new iteration
				Iterations ++;

				// Gather new 100 messages and go down from last one
				IReadOnlyList<DiscordMessage> AllMessage = await context.Channel.GetMessagesAsync (100, LastMessage);

				// Saving all message
				for (int i = 0; i < AllMessage.Count; i++) {
					ChatLog += AllMessage[i].CreationTimestamp.DateTime.ToString () + ";" + AllMessage[i].Id + ";" + AllMessage[i].Author.Username + ";" + AllMessage[i].Content + "\n";
					TotalMessages ++;
				}
				// Record ID of last message, for next iteration
				LastMessage = AllMessage[AllMessage.Count - 1].Id;

				// If message count low then 100, then this iteration last
				if (AllMessage.Count < 100) {
					AllSaved = true;
				}
				streamWriter.WriteLine (ChatLog);
				ChatLog = string.Empty;

				// Iteration result
				Console.WriteLine ("Iterations: " + Iterations + ", Total messages: " + TotalMessages);
			}

			streamWriter.Close();
		}
	}
}
