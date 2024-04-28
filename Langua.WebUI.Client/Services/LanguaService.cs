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
        [Inject] public NavigationManager navigationManager {  get; set; }
        [Inject] public Uri baseUri { get; set; }
        //private readonly NavigationManager navigationManager;
        //private readonly HttpClient httpClient;
        //private readonly Uri baseUri;
        public LangClientService()//NavigationManager navigation,Uri uri)
        {
           // navigationManager = navigation;
          //  httpClient = http;
         //   baseUri = uri;
        }


    }
}
