using Google.Protobuf;

using Langua.Models;
using Langua.Repositories.Interfaces;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Langua.WebUI.Pages.Chat
{
    public partial class AskAi
    {
        [Inject] public IAIService _aIService { get; set; }

        public string MessageToSend { get; set; }
        public string ResponseMessage { get; set; }
        List<IAMessage> Messages = new List<IAMessage>() { };
        bool waitingForResponse;
        public async Task SendClicked()
        {
            try
            {
                waitingForResponse = true;
                StateHasChanged();

                var reqData = new GeminiRequest
                {
                    contents = new Content[] {
                new Content {
                    Parts= new List<Part>(){
                        new Part{
                            Text = MessageToSend
                        }
                    }
                }
                    }
                };

                Messages.Add(new IAMessage { Message = MessageToSend, Sender = SenderAIMessage.User });
                MessageToSend = string.Empty;

                var result = await _aIService.AskGemini(reqData);
                if (result != null)
                {
                    ResponseMessage = result?.Candidates?.FirstOrDefault()?.Content.Parts?.FirstOrDefault()?.Text;
                    if (!string.IsNullOrEmpty(ResponseMessage))
                    {
                        var _ResAIMessage = new IAMessage { Message = "", Sender = SenderAIMessage.AI, Guid = new() };
                        Messages.Add(_ResAIMessage);

                        //_ResAIMessage.Message = ResponseMessage;
                        StateHasChanged();
                    }
                }
            }
            finally
            {
                await JSRuntime.InvokeVoidAsync("startTyping", "elementToTypeIn", ResponseMessage, 10);
                waitingForResponse = false;
                StateHasChanged();
            }
        }
    }
}