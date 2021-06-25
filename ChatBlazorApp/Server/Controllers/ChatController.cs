using ChatBlazorApp.Server.Data;
using ChatBlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBlazorApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
		private readonly PreviousChatArchive previousChatArchive;
        private readonly CosmosRepository cosmosRepository;

        public ChatController(PreviousChatArchive previousChatArchive, CosmosRepository cosmosRepository)
		{
			this.previousChatArchive = previousChatArchive;
            this.cosmosRepository = cosmosRepository;
        }

		[HttpGet("{roomName}")]
		public async Task<IEnumerable<ChatData>> Get([FromRoute] string roomName)
		{
			//check that the room exists
			if (previousChatArchive.Chats.ContainsKey(roomName))
				return previousChatArchive.Chats[roomName].Messages;
			//return new List<ChatData>();
			//if it isn't in the local look for it in the db
			var chatThread = (await cosmosRepository.GetAll<ChatThread>(roomName)).FirstOrDefault();
			return chatThread?.Messages ?? new List<ChatData>();

		}
	}
}
