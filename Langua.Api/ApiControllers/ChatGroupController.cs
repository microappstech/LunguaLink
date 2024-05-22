using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Langua.ApiControllers.LanguaHub;
using Langua.Repositories.Interfaces;
using Langua.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Langua.ApiControllers.ApiControllers
{
    [Route("api/ChatGroup")]
    [ApiController]
    public class ChatGroupController:ControllerBase
    {

        private HubConnection? ChatGroupHub;
        private readonly IConfiguration Config;
        private IRepositoryCrudBase<MessageGroup> repositoryCrud;
        public ChatGroupController(IRepositoryCrudBase<MessageGroup> crud, IConfiguration config)
        {
            this.repositoryCrud = crud;
            Config = config;
        }
        [HttpPost("SendeMessage")]
        public async Task<IActionResult> SendMessageToGroup(string groupId,string FromUserId, string Message)
        {
            try
            {
                var GroupMessage = new Models.MessageGroup()
                {
                    GroupId = int.Parse(groupId),
                    Content = Message,
                    SenderId = FromUserId,
                    SendAt = DateTime.Now
                };
                var res = repositoryCrud.Add(GroupMessage);
                if (res.Succeeded)
                {
                    var url = Config["ChatGroupEndPoint"] + ChatHub.ChatGroupEndPoint;
                    ChatGroupHub = new  HubConnectionBuilder()
                        .WithUrl(url)
                        .WithAutomaticReconnect()
                        .Build();
                    await ChatGroupHub.StartAsync();
                    await ChatGroupHub.InvokeAsync("SendMessageToGroup", groupId,FromUserId,Message);
                    return Ok();
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
