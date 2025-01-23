using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Components;
using Langua.WebUI.Pages.Teachers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Transactions;

namespace Langua.WebUI.Pages.Manager
{
    public partial class CUManager
    {
        [Parameter] public bool IsEdit { get; set; }
        [Inject] IRepositoryCrudBase<Models.Manager>? crudRepository { get; set; }
        [Inject] IRepositoryCrudBase<Models.Department>? DepRepository { get; set; }
        public Models.Manager? Manager { get; set; }
        [Parameter] public int? Id { get; set; }
        public IEnumerable<Department>?  Departements { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool DataReady { get; set; }
        public bool ChangePassword { get; set; }
        
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override Task OnInitializedAsync()
        {
            Errors = new List<string>();
            if(IsEdit || Id != null)
            {
                var resultM = crudRepository!.GetById((int)Id!);
                if(resultM.Succeeded)
                {
                    Manager = resultM.Value;
                }
            }
            else
            {
                Manager = new Models.Manager()
                {
                    FullName = "",
                    UserId ="",
                    Photo ="",
                    Password="",
                    Phone ="",
                    ConfirmPassword="",
                    Email=""
                };
            }
            var ResultDep = DepRepository!.GetAll();
            if (ResultDep.Succeeded)
            {
                Departements = ResultDep.Value.Where(d => d.Manager == null  
                || (d.Manager !=null && d.Manager.Id == Manager.Id)).ToList();
                
            }
            return base.OnInitializedAsync();
        }
        public async Task Delete(Models.Manager args)
        {
            if(await dialogService.Confirm("Confirmation suppression","Are you sure you want to delete this manager") == true)
            {
                var result = crudRepository!.Delete(args);
                if (result.Succeeded)
                {
                    NotifySuccess("success", "Suppression successful completed");
                }
            }
        }

        public async void loadImage(InputFileChangeEventArgs args)
        {
            MemoryStream memoryStream = new MemoryStream();
            await args.File.OpenReadStream().CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);

            Manager!.Photo = "data:image/png;base64," + base64;
        }
        public async Task HandleValidSubmit()
        {
            try
            {
                Errors = new List<string>();

                if (Manager.Id == 0 && !IsEdit)
                {
                    Manager.Password = Manager.Email.Substring(0, Manager.Email.IndexOf("@")) + "_" + DateTime.Now.Day;
                    ApplicationUser _user = new ApplicationUser()
                    {
                        Email = Manager.Email,
                        UserName = Manager.Email,
                        Password = Manager.Password,
                        NormalizedUserName = Manager.FullName,
                        PhoneNumber = Manager.Phone,
                        FullName = Manager.FullName,
                    };

                    var TaskUser = await Security!.RegisterUser(_user);
                    if (TaskUser.Succeeded)
                    {
                        var role = await Security!.AddRoleToUser(TaskUser.Value, "MANAGER");
                        var result = crudRepository!.Add(Manager);
                        if (result.Succeeded)
                        {
                            Notify("Success", "Item created successfully", Radzen.NotificationSeverity.Success);
                            dialogService.Close();
                        }
                        else
                        {
                            Errors.Add(result.Error);
                            return; // Ensure we exit to avoid calling Complete() and dispose
                        }
                    }
                    else
                    {
                        Errors.Add(TaskUser.Error);
                        return; // Ensure we exit to avoid calling Complete() and dispose
                    }
                }
                else
                {
                    var result = crudRepository.Update(Manager);
                    if (result.Succeeded)
                    {
                        Notify("Success", "Item updated successfully", Radzen.NotificationSeverity.Success);
                        dialogService.Close();
                    }
                    else
                    {
                        Errors.Add(result.Error);
                        return; // Ensure we exit to avoid calling Complete() and dispose
                    }
                }
            }
            catch (Exception ex)
            {
                Notify("Error", "Something went wrong! Please try again", Radzen.NotificationSeverity.Error);
            }
        }

        public async Task HandleValidSubmit2()
        {
            using (var scope = new TransactionScope( TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    Errors = new();
            
                    if (Manager.Id == 0 && !IsEdit)
                    {
                        Manager.Password = Manager.Email.Substring(0, Manager.Email.IndexOf("@"))+"_"+DateTime.Now.Day;
                        ApplicationUser _user = new ApplicationUser()
                        {
                            Email = Manager.Email,
                            UserName = Manager.Email,
                            Password = Manager.Password,
                            NormalizedUserName = Manager.FullName,
                            PhoneNumber = Manager.Phone,
                            FullName= Manager.FullName,
                        };
                        var TaskUser = await Security!.RegisterUser(_user);
                        if (TaskUser.Succeeded)
                        {
                            var role = await Security!.AddRoleToUser(TaskUser.Value, "MANAGER");//.ConfigureAwait(false);
                            var result = crudRepository!.Add(Manager);
                            if (result.Succeeded)
                            {
                                Notify("Success", "Item created successfully", Radzen.NotificationSeverity.Success);
                                dialogService.Close();
                            }
                            else
                            {
                                Errors.Add(result.Error);
                                return;
                            }
                        }
                        else
                        {
                            Errors.Add(TaskUser.Error);
                        }
                        scope.Complete();
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        var result = crudRepository.Update(Manager);
                        if (result.Succeeded)
                        {
                            Notify("Success", "Item updated successfully", Radzen.NotificationSeverity.Success);
                            
                            dialogService.Close();

                        }
                        else
                        {
                            Errors.Add(result.Error);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Notify("Error", "Somethinf went wrong! Please try again",Radzen.NotificationSeverity.Error);
                }
            }
        
        }
    }
}