using Langua.Models;
using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Client.Pages.Rooms
{
    public partial class RoomComponent : BasePageClient
    {
        [Parameter] public string SessionId { get; set; }
        public Session Session { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            if(int.TryParse(SessionId, out int _sessionId))
            {

                Session = await LangClientService!.GetSessionById(id: _sessionId);
                if(Session is null)
                {
                    Notify("Error", "You are unable to join to this session", Radzen.NotificationSeverity.Error);
                }
            }

        }

    }
}