using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Langua.WebUI.Client.Pages
{
    public partial class Component:BasePageClient
    {
        private ElementReference _codeTextArea;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JSRuntime.InvokeVoidAsync("initializeCodeMirror", _codeTextArea);
            }
        }

        public void Dispose()
        {
            // Clean up resources if necessary
        }
    }
}