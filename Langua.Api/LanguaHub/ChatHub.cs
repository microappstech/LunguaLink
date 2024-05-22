using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.ApiControllers.LanguaHub
{
    public class ChatHub : Hub
    {
        public static string ChatGroupEndPoint = "ChatGroup";
        public Task SendMessageToGroup(string FromUserId, string GroupId,string Message)
        {
            return Clients.All.SendAsync("SendMessageToGroup", FromUserId, GroupId, Message);
        }
        public Task SendMessageToUser(string ToUserId, string FromUserId, string Message)
        {
            return Clients.User(ToUserId).SendAsync("SendMessage",Message);
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
