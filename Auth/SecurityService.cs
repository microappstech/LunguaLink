using Langua.DataContext.Data;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Langua.Auth
{
    public partial class SecurityService
    {
        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IEnumerable<IdentityError> identityErrors;
        private ILogger<SecurityService> Ilogger;
        private NavigationManager navigationManager;
        private string? errorMessage ;
        public SecurityService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<SecurityService> logger,
            NavigationManager manager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            Ilogger =  logger;
            navigationManager = manager;
            
        }
        public ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }
        public async Task<ApplicationUser> RegisterUser(ApplicationUser ApplicationUser)
        {
            var user = CreateUser();

            //await UserStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            //var emailStore = GetEmailStore();
            //await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, ApplicationUser.Password);

            if (!result.Succeeded)
            {
                identityErrors = result.Errors;
                return null;
            }

            Ilogger.LogInformation($"User created a new account with password. on : {DateTime.Now}");

            var userId = await _userManager.GetUserIdAsync(user);
            #region Cancel
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //var callbackUrl = NavigationManager.GetUriWithQueryParameters(
            //NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
            //new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });

            //await EmailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

            //if (UserManager.Options.SignIn.RequireConfirmedAccount)
            //{
            //    RedirectManager.RedirectTo(
            //        "Account/RegisterConfirmation",
            //        new() { ["email"] = Input.Email, ["returnUrl"] = ReturnUrl });
            //}
            #endregion Cancel
            await _signInManager.SignInAsync(user, isPersistent: false);
            return await Task.FromResult(user);
        }

        public async Task Login(ModelView.InputModels.LoginInput Input)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                Ilogger.LogInformation("User logged in.");
                RedirectTo(ReturnUrl);
            }
            else if (result.RequiresTwoFactor)
            {
                RedirectTo(
                    "Account/LoginWith2fa",
                    new() { ["returnUrl"] = ReturnUrl, ["rememberMe"] = Input.RememberMe });
            }
            else if (result.IsLockedOut)
            {
                Ilogger.LogWarning("User account locked out.");
                RedirectTo("Account/Lockout");
            }
            else
            {
                errorMessage = "Error: Invalid login attempt.";
            }
        }



        public void RedirectToLogin()
        {
            var uriWithoutQuery = navigationManager.ToAbsoluteUri("/Account/Login").GetLeftPart(UriPartial.Path);

            RedirectTo("/");
        }
        public void RedirectTo(string uri, Dictionary<string, object?> queryParameters=null)
        {
            uri ??= "";
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = navigationManager.ToBaseRelativePath(uri);
            }
            var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
            if(queryParameters is null)
            {
                RedirectTo(uriWithoutQuery);
            }
            else
            {
                var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
                RedirectTo(newUri);
            }
        }
        public void RedirectTo(string? uri)
        {
            uri ??= "";

            // Prevent open redirects.
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = navigationManager.ToBaseRelativePath(uri);
            }

            navigationManager.NavigateTo(uri);
            throw new InvalidOperationException($"");
        }
    }
}
