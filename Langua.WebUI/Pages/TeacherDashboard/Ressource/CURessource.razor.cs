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
        [Parameter] public int Id { get; set; }
        public bool IsEdit { get; set; }
        public EditContext? editContext;
        public string? url { get; set; }
        public string? UrlVedio { get; set; }
        RadzenUpload? upload;
        RadzenUpload? uploadDD;
        public Langua.Models.Ressource? Ressource { get; set; }
        [Inject] public Langua.Repositories.Interfaces.IRepositoryCrudBase<Models.Ressource>? repositoryRessource { get; set; }
        [Inject] HttpClient? Http { get; set; }
        public string? FileName { get; set; }
        public void OnError(UploadErrorEventArgs err)
        {

        }

        


        async Task OnComplete(UploadChangeEventArgs args)
        {
             await upload!.Upload();
        }
        


        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (Id is not 0 )
                {

                    var resulRes = repositoryRessource!.GetById(Id);
                    if (resulRes.Succeeded)
                    {
                        IsEdit = true;
                        Ressource = resulRes.Value;
                    }
                    else
                    {
                        Ressource = new Models.Ressource();
                    }
                }
                else
                {
                    Ressource = new Models.Ressource()
                    {
                        RessourceType = (int)RessourceType.URL
                    };
                    editContext = new EditContext(new Models.Ressource());
                }
            }
            catch
            {
                Notify(L["Error"], L["Something happend wrong"], NotificationSeverity.Error);
            }
            

        }

        public async Task Submit()
        {
            try
            {
                switch (Ressource!.RessourceType)
                {
                    case (int)RessourceType.URL:
                        //Ressource.Url = url;
                        Ressource.ContentBytes = null;
                        break;
                    case (int)RessourceType.VEDIO:
                        //Ressource.Url = UrlVedio;
                        Ressource.ContentBytes = null;
                        break;
                    case (int)RessourceType.File:
                        //var ileBytes = await Http!.GetByteArrayAsync($"{LanguaUrl}/api/Upload/");
                        //this.Ressource.ContentBytes = ileBytes;
                        var base64 = await Http.GetStringAsync($"{LanguaUrl}/api/Upload/Base64");
                        this.Ressource.ContentFile = base64;
                        break;
                }
                Ressource.TeacherId = Security!.Teacher.Id;
                
                var ResulCrRessource = IsEdit? repositoryRessource!.Update(Ressource) : repositoryRessource!.Add(Ressource);
                if (ResulCrRessource.Succeeded)
                {
                    dialogService!.Close(true);
                    Notify("Success", "Ressource Created Successfuly", NotificationSeverity.Success);
                }
                else
                {
                    Notify(L["Error"], L["Creation finished with errors"], NotificationSeverity.Error);
                }
            }
            catch
            {
                Notify(L["Error"], L["Creation finished with errors"], NotificationSeverity.Error);
            }
            
        }
        
    }
}