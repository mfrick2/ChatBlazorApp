using ChatBlazorApp.Server.Data;
using ChatBlazorApp.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBlazorApp.Server.Hubs
{
    public class ChatHub : Hub
    {
		private readonly PreviousChatArchive previousChatArchive;
        private readonly CosmosRepository cosmosRepository;

        public ChatHub(PreviousChatArchive previousChatArchive, CosmosRepository cosmosRepository)
		{
			this.previousChatArchive = previousChatArchive;
            this.cosmosRepository = cosmosRepository;
        }

		//method to join the current web connection to the rooms group
		public async Task JoinRoom(string roomName)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
		}

		//task for sending and recieving messages.
        public async Task SendMessage(string user, string message, string roomName)
		{
			//the message; who said what
			ChatData chatData = new Shared.ChatData { User = user, Message = message };

			ChatThread chatThread = new ChatThread();
			chatThread.RoomName = roomName;
			chatThread.Id = Guid.NewGuid().ToString();


			//Check if there is already a list of chat data for the room
			if (previousChatArchive.Chats.ContainsKey(roomName))
			{
				chatThread = previousChatArchive.Chats[roomName];
			}
			//if there isn't then create a new one
			else
			{
				previousChatArchive.Chats.Add(roomName,  chatThread);
			}
			chatThread.Messages.Add(chatData);

			//puts the item in the db
			await cosmosRepository.Upsert(chatThread);

			//Send the message filtered by the group from signalr unique to each room
            await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message, roomName);
		}
    }
}
