using BenchmarkDotNet.Validators;
using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.Transactions;

namespace Langua.WebUI.Pages.Candidates
{
    public partial class AddCandidateComponent : BasePage
    {
        protected string fileName;
        protected long? fileSize;
        public bool Submited { get; set; }
        [Inject] private IRepositoryCrudBase<Candidat>? _repository { get; set; }
        [Inject] private IRepositoryCrudBase<Subject>? _repositorySubjects { get; set; }

        protected Candidat? candidate { get; set; }
        public IEnumerable<Subject>? subjects { get; set; }
        public IEnumerable<Department>? Departments { get; set; }
        public bool DataReady { get; set; }
        public string? ErrorMail { get; set; }
        public List<string> Errors { get; set; } = new ();
        protected override async Task OnInitializedAsync()
        {
            candidate = new Candidat();
            var SubjectResult = _repositorySubjects!.GetAll();
            var resultDeparts = await LanguaService.GetDepartement();
            if (resultDeparts.Succeeded)
            {
                Departments = resultDeparts.Value;
            }
            if (SubjectResult.Succeeded)
            {
                subjects = SubjectResult.Value;
            }
            
        }
        public void ValidateEmail(string email)
        {
            ErrorMail = string.Empty;
            if (!string.IsNullOrEmpty(email) && email.ToLower().Contains("@gmail.com")==false) 
            {
                ErrorMail = L["Email Should be account google"];
            }
        }
        protected async Task HandleValidSubmit()
        {
            Submited = true;
            ErrorMail = "";
            if (!string.IsNullOrEmpty(candidate?.Email) && !candidate.Email.Contains("@gmail.com"))
            {
                ErrorMail = L["Email Should be account google"];
                return;
            }
            candidate.Password = candidate.Email.Substring(0, candidate.Email.IndexOf("@")) + "_" + DateTime.Now.Day;
            ApplicationUser _user = new ApplicationUser()
            {
                Email = candidate.Email,
                UserName = candidate.Email,
                Password = candidate.Password,
                NormalizedUserName = candidate.FullName,
                PhoneNumber = candidate.Phone,
                FullName = candidate.FullName,
                EmailConfirmed = false,
                Code= GenerateVerificationCode(),

            };

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TimeSpan.FromMinutes(3), TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var taskResult = await Security!.RegisterUser(_user);

                    if (taskResult.Succeeded)
                    {
                        var sendVerifyMail = await mailService!.SendVerificationCode(candidate.Email,candidate.FullName,_user.Code);
                        var r = await Security.AddRoleToUser(taskResult.Value, "CANDIDATE");
                        candidate.UserId = taskResult.Value.Id;
                        var result = _repository.Add(candidate);
                        if (result.Succeeded)
                        {
                            notificationService.Notify(NotificationSeverity.Success, "Creation Successful Completed");
                            StateHasChanged();
                            dialogService.Close(null);
                            
                        }
                        else
                        {
                            Notify(L["Failed"], L["Something wrong"],NotificationSeverity.Error);
                        }
                    }
                    else
                    {
                        Errors.Add(taskResult.Error);
                    }

                }catch(Exception ex)
                {
                
                }
                
            }
            Submited = false;
        }
        public void Close()
        {
            dialogService.Close(null);
        }
        public async void loadImage(InputFileChangeEventArgs args)
        {
            MemoryStream memoryStream = new MemoryStream();
            await args.File.OpenReadStream().CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);
            
            candidate.Photo = "data:image/png;base64," + base64;

        }
        public void OnChange(string value, string name)
        {

        }

        public void OnError(UploadErrorEventArgs args, string name)
        {
            Notify(L["Error"], args.Message, NotificationSeverity.Error);
        }
    }
}