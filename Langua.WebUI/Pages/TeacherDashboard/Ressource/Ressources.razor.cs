using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Radzen;
using Langua.Models;

namespace Langua.WebUI.Pages.TeacherDashboard.Ressource
{
    public partial class Ressources : BasePage
    {
        [Inject] public Langua.Repositories.Interfaces.IRepositoryCrudBase<Models.Ressource> repositoryRessource { get; set; } = null!;
        private RadzenDataGrid<Models.Ressource> ResGrid = null!;
        public List<Models.Ressource> Resources { get; set; } = null!;
        public Models.Ressource ResToEdit { get; set; } = null!;
        //public string url { get; set; } = null!;
        public string base64 { get; set; } = null!;
        public async Task RddRessource()
        {
            var result = await dialogService.OpenAsync<CURessource>(L["Add Contenu"], null);
            await LoadRessourcesForTeacher();
            await ResGrid.Reload();
        }
        int _SizeGridColumn = 12;
        int _SizePreviewColumn = 0;
        bool _PreviewColumnVisible = false;
        protected override async Task OnInitializedAsync()
        {
            await Security!.InitializedTeacher();
            await LoadRessourcesForTeacher();
        }
        public async Task LoadRessourcesForTeacher()
        {
            var resRessources = await LanguaService.GetRessourceByTeacherId(Security.Teacher.Id);
            if (resRessources.Succeeded)
            {
                Resources = resRessources.Value.ToList();
                Resources.ToList()?.ForEach(res =>
                {
                    res.GroupRessources.ToList()?.ForEach(GR =>
                    {
                        res.GroupsName += $"{GR.Group?.Name}, ";
                    });
                });
            }
        }
        public async Task Delete(Models.Ressource ressource)
        {
            if (await Confirm(L["Confirm Delete"], L["Are you sure want to delete this item"]) == true)
            {
                var DeleteRes = repositoryRessource.Delete(ressource);
                if (DeleteRes.Succeeded)
                {
                    await LoadRessourcesForTeacher();

                    Notify(L["Successful"], L["The Item successful deleted"], NotificationSeverity.Success);
                }
            }
        }


        private async Task Preview(Models.Ressource r)
        {
            switch ((RessourceType)r.RessourceType)
            {
                case RessourceType.File:
                    _SizePreviewColumn = 6;
                    _SizeGridColumn = 6;
                    _PreviewColumnVisible = true;
                    //url = $"data:application/pdf;base64,{Convert.ToBase64String(r.ContentBytes, 0, r.ContentBytes.Length)}";
                    base64 = $"data:application/pdf;base64,{r.ContentFile}";
                    break;

                case RessourceType.VEDIO:
                case RessourceType.URL:
                    await JSRuntime.InvokeVoidAsync("open", r.Url, "_blank");
                    break;
            }
        }

        public async Task Publish(Models.Ressource ressource)
        {
            var result = await dialogService!.OpenAsync<PublishRessource>(L["Publish contenu to groupes"], new Dictionary<string, object> { { "ressource", ressource } });
            if (result == true)
                await ResGrid.Reload();
        }
        public async Task Edit(Models.Ressource ressource)
        {
            ResToEdit = ressource;
            var result = await dialogService!.OpenAsync<CURessource>(L["Edit The Ressource"], new Dictionary<string, object> { { "Id", ressource.Id } });
            await LoadRessourcesForTeacher();
        }
    }
}