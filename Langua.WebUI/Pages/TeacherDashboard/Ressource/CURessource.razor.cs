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
        public string FileName { get; set; }
        public void OnError(UploadErrorEventArgs err)
        {

        }

        


        async Task OnComplete(UploadChangeEventArgs args)
        {
             await upload.Upload();
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
            try
            {
                switch (Ressource!.RessourceType)
                {
                    case (int)RessourceType.URL:
                        Ressource.Url = url;
                        Ressource.ContentBytes = null;
                        break;
                    case (int)RessourceType.VEDIO:
                        Ressource.Url = UrlVedio;
                        Ressource.ContentBytes = null;
                        break;
                    case (int)RessourceType.File:
                        var ileBytes = await Http.GetByteArrayAsync("https://localhost:44317/api/Upload/");
                        var base64 = await Http.GetStringAsync("https://localhost:44317/api/Upload/Base64");
                        this.Ressource.ContentFile = base64;
                        this.Ressource.ContentBytes = ileBytes;
                        break;
                }
                var ResulCrRessource = repositoryRessource.Add(Ressource);
                if (ResulCrRessource.Succeeded)
                {
                    Notify("Success", "Ressource Created Successfuly", NotificationSeverity.Success);
                    dialogService.Close(null);
                }
            }
            catch(Exception ex)
            {

            }
            
        }
        
    }
}