using Langua.DataContext.Data;
using Langua.Shared.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Langua.Account
{
    public class SecurityService
    {
        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IEnumerable<IdentityError> identityErrors;
        private ILogger<SecurityService> Ilogger;
        private NavigationManager navigationManager;
        private string? errorMessage;
        private readonly AuthenticationStateProvider authentication;
        private readonly IWebHostEnvironment webHost;
        private readonly RoleManager<IdentityRole> roleManager;
        public SecurityService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<SecurityService> logger, AuthenticationStateProvider authentication,
            NavigationManager manager, IWebHostEnvironment webHost, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            Ilogger = logger;
            navigationManager = manager;
            this.authentication = authentication;
            this.webHost = webHost;
            this.roleManager = roleManager;
        }
        public ClaimsPrincipal Principal { get; set; }
        private ApplicationUser user;
        //private AuthenticationStateProvider authenticationStateProvider;

        public ApplicationUser User { get { return user; } }
        public async Task IsAuthenticated()
        {
            var r = await authentication.GetAuthenticationStateAsync();
            if (!r.User.Identity.IsAuthenticated)
            {
                RedirectTo("/login");
            }
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
            user.Email = ApplicationUser.Email;
            user.UserName = ApplicationUser.Email;
            user.Password = ApplicationUser.Password;
            user.NormalizedUserName = ApplicationUser.NormalizedUserName;
            user.PhoneNumber = ApplicationUser.PhoneNumber;
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

            //var userId = await _userManager.GetUserIdAsync(user);
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
            return await Task.FromResult(user);
        }

        public async Task<EResult> Login(ModelView.InputModels.LoginInput Input)
        {
            if (Input.Email == "admin@langua.ma" && Input.Password == "admin" && webHost.EnvironmentName == "Development")
            {
                var RoleAdmin = await roleManager.FindByNameAsync("ADMIN");
                if (RoleAdmin == null)
                {
                    var r = await roleManager.CreateAsync(new IdentityRole("ADMIN"));
                }
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,"Admin"),
                    new Claim(ClaimTypes.Email,"admin@langua.ma"),
                    new Claim(ClaimTypes.Role ,"ADMIN")
                };
                //roleManager.Roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.Name)));
                var us = new ApplicationUser { Email = Input.Email, Password = Input.Password };
                await _signInManager.SignInAsync(us, isPersistent: false);
                await _userManager.AddToRoleAsync(us, "ADMIN");
                return new EResult(true, "is dev env");
            }
            else
            {


                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user != null)
                {
                    if (await _signInManager.CanSignInAsync(user))
                    {

                        Ilogger.LogInformation($"Login : username: {Input.Email}");

                        var result = await _signInManager.PasswordSignInAsync(user, Input.Password, isPersistent: false, false);
                        if (result.Succeeded)
                        {
                            Ilogger.LogInformation("User logged in.");
                            return new EResult(true);
                        }
                        else if (result.RequiresTwoFactor)
                        {
                            return new EResult(true);
                        }
                        else if (result.IsLockedOut)
                        {
                            Ilogger.LogWarning("User account locked out.");
                            return new EResult(false, "Your Account Is Locked");
                        }
                        else
                        {
                            return new EResult(true, "Invalid login attempt.");

                        }
                    }
                    else
                    {
                        return new EResult(false);
                    }
                }
                else
                {
                    Ilogger.LogInformation("Login : No account with this mail : " + Input.Email);
                    return new EResult(false, "No User With This Name");
                }

            }
        }



        public void RedirectToLogin()
        {
            var uriWithoutQuery = navigationManager.ToAbsoluteUri("/Account/Login").GetLeftPart(UriPartial.Path);

            RedirectTo("/");
        }
        public void RedirectTo(string uri, Dictionary<string, object?> queryParameters = null)
        {
            uri ??= "";
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = navigationManager.ToBaseRelativePath(uri);
            }
            var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
            if (queryParameters is null)
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
        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            var result = roleManager.Roles.ToList();
            return await Task.FromResult(result);
        }
        public async Task Logut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser> GetById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                return user;
            }
            return null;
        }
    }
}
