using Langua.DataContext.Data;
using Langua.Shared.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Linq.Expressions;
using Langua.Models;

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

        public ApplicationUser User { get { return user; } }
        public async Task<bool> InitializeAsync()
        {
            var logged = await authentication.GetAuthenticationStateAsync();
            if(logged.User?.Identity?.Name != null && user == null)
            {
                Principal = logged.User;
                var us = await _userManager.FindByNameAsync(logged.User.Identity.Name);
                if(us is not null)
                {
                    user = us;
                    user.Roles = await _userManager.GetRolesAsync(us);
                }
            }
            return logged.User.Identity.IsAuthenticated;
        }
        public  bool IsAuthenticated()
        {
            return Principal !=null ? Principal.Identity.IsAuthenticated :false;
        }
        public async Task IsAuthenticatedWidthRedirect()
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
            var result = await _userManager.CreateAsync(user, ApplicationUser.Password);
            if (!result.Succeeded)
            {
                identityErrors = result.Errors;
                return null;
            }
            Ilogger.LogInformation($"User created a new account with password. on : {DateTime.Now}");
            return await Task.FromResult(user);
        }
        public async Task<(bool,string)> CreateRole(string roleName)
        {
            
            var roleTask = await roleManager.CreateAsync(new IdentityRole { Name = roleName });
            if (roleTask.Succeeded)
                return (true, "");
            return (false, "something wrong try again");
        }
        public async Task<bool> AddRoleToUser(ApplicationUser us, string role)
        {
            var existRole = await roleManager.RoleExistsAsync(role);
            try
            {
                if(!existRole)
                    await CreateRole(role);
                var result = await _userManager.AddToRoleAsync(us, role);
                return result.Succeeded;
            }
            catch 
            {
                return false;
            }
        }
        public async Task<LResult> Login(ModelView.InputModels.LoginInput Input)
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
                var us = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Password = Input.Password };
                await _signInManager.SignInWithClaimsAsync(new ApplicationUser { UserName = Input.Email, Email = Input.Email }, isPersistent: false, claims);
                return new LResult(true, "is dev env",uri:"/");
            }
            else
            {
                var us = await _userManager.FindByEmailAsync(Input.Email);
                if (us != null)
                {
                    Ilogger.LogInformation($"Login : username: {Input.Email}");
                    SignInResult? result = await _signInManager.PasswordSignInAsync(us, Input.Password, isPersistent: false, false);
                    if (result.Succeeded)
                    {
                        Ilogger.LogInformation("User logged in.");
                        LResult loginResult = new LResult(true) { };
                        user = us;
                        var roles = await _userManager.GetRolesAsync(user);
                        user.Roles = roles;
                        if (roles.Contains("ADMIN"))
                            return new LResult(true, uri: "/");
                        if (roles.Contains("TEACHER"))
                            return new LResult(true, uri: "/Teacher");
                        if (roles.Contains("MANAGER"))
                            return new LResult(true, uri: "/Manager");
                        return new LResult(true, uri: "NotFound");
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        return new LResult(false, "You need to confirm your login using two factor");
                    }
                    else if (result.IsLockedOut)
                    {
                        Ilogger.LogWarning("User account locked out.");
                        return new LResult(false, "Your Account Is Locked");
                    }
                    else
                    {
                        return new LResult(false, "Invalid login attempt.");
                    }
                }
                else
                {
                    Ilogger.LogInformation("Login : No account with this mail : " + Input.Email);
                    return new LResult(false, "No User With This Name");
                }

            }
        }

        public async Task<bool> IsInRole(string role)
        {
            var existRole = User.Roles.Contains(role);
            return await Task.FromResult(existRole);
        }

        public void RedirectToLogin()
        {
            var uriWithoutQuery = navigationManager.ToAbsoluteUri("/Login").GetLeftPart(UriPartial.Path);

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
