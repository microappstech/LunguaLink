using Google.Protobuf;

using Langua.Models;
using Langua.Repositories.Interfaces;

using Microsoft.AspNetCore.Components;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.JSInterop;

namespace Langua.WebUI.Pages.Chat
{
    public partial class AskAi
    {
        [Inject] public IAIService _aIService { get; set; }
        private DotNetObjectReference<AskAi> objRef;

        protected override void OnInitialized()
        {
            objRef = DotNetObjectReference.Create(this);
        }
        public string MessageToSend { get; set; }
        public string ResponseMessage { get; set; }
        List<IAMessage> Messages = new List<IAMessage>() { };
        bool waitingForResponse, isTyping;
        public async Task ButtonClicked()
        {
            if (!waitingForResponse)
                await SendClicked();
            else
               await StopClicked();
        }
        public async Task StopClicked()
        {
            isTyping = false;
            await JSRuntime.InvokeVoidAsync("stopTyping");
            Messages.OrderBy(i => i.Sender).LastOrDefault().Message = ResponseMessage;
        }
        public async Task SendClicked()
        {
            try
            {
                ResponseMessage = string.Empty;
                waitingForResponse = true;
                StateHasChanged();

                var reqData = new GeminiRequest
                {
                    contents = new Content[] {
                        new Content {
                            Parts= new Part[]
                            {
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
                    }
                }
                        StateHasChanged();
            }catch(Exception ex)
            {
                Messages.Remove(Messages.OrderBy(Message => Message.SentAt).LastOrDefault());//.OrderBy(Message => Message.SentAt);
                NotifyError(L["Error"], ex.Message);
            }
            finally
            {
                waitingForResponse = false;
                isTyping = true;
                JSRuntime.InvokeVoidAsync("startTyping", "elementToTypeIn", ResponseMessage, 5, objRef);
                StateHasChanged();
            }
        }
        [JSInvokable("OnTypingFinished")]
        public void TypingFinished(string elementId)
        {
            isTyping = false;
            StateHasChanged();
            //return;
            if (!string.IsNullOrEmpty(ResponseMessage))
                Messages.OrderBy(i => i.Sender).LastOrDefault().Message = ResponseMessage;
        }
        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}