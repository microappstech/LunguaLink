using Langua.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Langua.Repositories.Interfaces;
using Langua.DataContext.Data;
using Radzen;
using System.Transactions;
using System.Data.Common;

namespace Langua.WebUI.Pages.Teachers
{
    public partial class AddTeacherComonent:BasePage
    {
        [Inject] private IRepositoryCrudBase<Teacher>? _repository { get; set; }
        [Inject] private IRepositoryCrudBase<Subject>? _repositorySubjects { get; set; }

        public List<string> Errors { get; set; } = new();
        protected Teacher? teacher { get; set; }
        public IEnumerable<Subject>? subjects { get; set; }
        public bool DataReady { get; set; }
        
        protected override async Task OnInitializedAsync()
        {

            teacher = new Teacher();
            var SubjectResult = _repositorySubjects!.GetAll();
            if (SubjectResult.Succeeded)
            {
                subjects = SubjectResult.Value;
            }
            base.OnInitialized();
            await base.OnInitializedAsync();
        }
        protected async Task HandleValidSubmit()
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                try
                {
                    teacher!.Password = teacher.Email.Substring(0, teacher!.Email!.IndexOf("@")) + "_" + DateTime.Now.Day;
                    ApplicationUser _user = new ApplicationUser()
                    {
                        Email = teacher.Email,
                        FullName=teacher.FullName,
                        UserName = teacher.Email,
                        Password = teacher.Password,
                        NormalizedUserName = teacher.FullName,
                        PhoneNumber = teacher.Phone
                    };
                    var TaskUser = await Security!.RegisterUser(_user);
                    if (TaskUser.Succeeded)
                    {
                        teacher.UserId = TaskUser.Value.Id;
                        var r = await Security.AddRoleToUser(TaskUser.Value, "TEACHER");
                        var result = _repository!.Add(teacher);
                        if (result.Succeeded)
                        {
                            Notify("Success", "Creation Successful Completed", NotificationSeverity.Success);
                            scope.Complete();
                            dialogService.Close(null);
                        }
                        else
                        {
                            Notify("Failed", "Something Wrong", NotificationSeverity.Error);
                        }
                    }
                    else
                    {
                        Errors.Add(TaskUser.Error);
                    }
                        
                }
                catch
                {
                    Notify("Failed", "Something Wrong", NotificationSeverity.Error);
                }
            }
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

            teacher.Photo = "data:image/png;base64," + base64;

        }
    }
}