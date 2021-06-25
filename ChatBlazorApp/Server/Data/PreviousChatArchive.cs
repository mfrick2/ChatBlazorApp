using ChatBlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatBlazorApp.Server.Data
{
    public class PreviousChatArchive
    {
        //dictionary to keep seperate lists for seperate rooms
        public Dictionary<string, ChatThread> Chats { get; set; } = new Dictionary <string, ChatThread>();
    }

    //formatting for the db
    public class ChatThread
    {
        //fields are in cammle case in the db so we have to rename them
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("roomName")]
        public string RoomName { get; set; }
        [JsonPropertyName("messages")]
        public List<ChatData> Messages { get; set; } = new List<ChatData>();
    }
}
