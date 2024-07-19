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
using Langua.Api;
using Langua.DataContext.Data;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Langua.ApiControllers.ApiControllers
{
    [Route("api/ChatGroup")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatGroupController : ControllerBase
    {
        private HubConnection? ChatGroupHub;
        private readonly IConfiguration Config;
        private IRepositoryCrudBase<MessageGroup> repositoryCrud;
        private LanguaContext context;
        public ChatGroupController(IRepositoryCrudBase<MessageGroup> crud, IConfiguration config,LanguaContext langua)
        {
            this.repositoryCrud = crud;
            Config = config;
            context = langua;
        }

        [HttpGet("GetMessages")]
        public async Task<ApiResponse<List<MessageResponse>>> GetMessages(int GroupId)
        {
            try
            {
                var respos = context.MessageGroups.Where(g => g.GroupId == GroupId).ToList();
                var ResponseData = respos.Select(i => new MessageResponse
                {
                    Id = i.Id,
                    MessageContent = i.Content,
                    Color = i.Color,
                    IsFile = i.ContentMessage != null,
                    ContentFile = i.ContentMessage,
                    GroupId = i.GroupId,
                    SuccessSended = i.SuccessSended
                }).ToList();
                return new ApiResponse<List<MessageResponse>> { Success = true, Data = ResponseData };
            }catch(Exception ex)
            {
                return new ApiResponse<List<MessageResponse>> { Data = null, Message = ex.Message, Success = false };
            }
        }

        [HttpPost("SendeMessage")]
        public async Task<IActionResult> SendMessageToGroup([FromBody]ApiSendedMessage message)
        {
            ApiSendedMessageState state = new ApiSendedMessageState();
            state.Sended = false;
            try
            {
                var GroupMessage = new Models.MessageGroup()
                {
                    GroupId = message.GroupId,
                    Content = message.Message,
                    SenderId = message.UserId,
                    SendAt = DateTime.Now
                };
                var res = repositoryCrud.Add(GroupMessage);
                if (res.Succeeded)
                {
                    state.Sended = true;
                    var url = Config["ChatGroupEndPoint"] + ChatHub.ChatGroupEndPoint;
                    ChatGroupHub = new  HubConnectionBuilder()
                        .WithUrl(url)
                        .WithAutomaticReconnect()
                        .Build();
                    await ChatGroupHub.StartAsync();
                    await ChatGroupHub.InvokeAsync("SendMessageToGroup", message.GroupId,message.UserId,message.Message);
                    state.Recieved = true;
                    return Ok(state);
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                state.Recieved = false;
                state.Detail = ex.Message;
                return BadRequest(state);
            }

        }

    }
}
