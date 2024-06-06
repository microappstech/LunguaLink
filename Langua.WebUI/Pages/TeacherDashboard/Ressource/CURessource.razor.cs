using Langua.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using Radzen;
using Radzen.Blazor;


namespace Langua.WebUI.Pages.TeacherDashboard.Ressource
{
    public partial class CURessource
    {
        public EditContext? editContext;
        public string? url { get; set; }
        public string FileContent { get; set; }
        public string? UrlVedio { get; set; }
        RadzenUpload? upload;
        RadzenUpload? uploadDD;
        public Langua.Models.Ressource? Ressource { get; set; }
        [Inject] public Langua.Repositories.Interfaces.IRepositoryCrudBase<Models.Ressource>? repositoryRessource { get; set; }
        [Inject] HttpClient Http { get; set; }
        public void OnError(UploadErrorEventArgs err)
        {

        }

        


        async Task UploadFileToServer(byte[] fileContent)
        {
            using var client = new HttpClient();
            var content = new MultipartFormDataContent();
            var fileContentStream = new MemoryStream(fileContent);
            content.Add(new StreamContent(fileContentStream), "file", "filename");

            var response = await client.PostAsync("https://yourserver/upload", content);

            // Handle response from server
            if (response.IsSuccessStatusCode)
            {
                // File uploaded successfully
            }
            else
            {
                // Handle error
            }
        }

        void OnChangeFile(string args, string name)
        {

        }

        async void OnComplete(UploadCompleteEventArgs args)
        {
           await upload.Upload();
            var ileBytes = await Http.GetByteArrayAsync("https://localhost:44317/api/Upload");
            this.Ressource.ContentBytes = ileBytes;
        }
        


        protected override async Task OnInitializedAsync()
        {
            this.Ressource = new Models.Ressource()
            {
                RessourceType = (int)RessourceType.URL
            };
            editContext = new EditContext(new Models.Ressource());

        }

        public async Task Submit()
        {
            switch (Ressource!.RessourceType)
            {
                case (int)RessourceType.URL:
                    Ressource.Url = url;
                    break;
                case (int)RessourceType.VEDIO:
                    Ressource.Url = UrlVedio;
                    break;
                case (int)RessourceType.File:
                    var bt = FileContent;
                    break;


                // how to read here 
            }
        }
        
    }
}