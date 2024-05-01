using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Client.Services
{
    public partial class LangClientService
    {
        //[Inject] public NavigationManager navigationManager {  get; set; }
        //[Inject] public Uri baseUri { get; set; }
        private readonly Uri baseUri;
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;
        public LangClientService(HttpClient httpClient, NavigationManager navigationManager, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/Langua/");
        }


    }
}
