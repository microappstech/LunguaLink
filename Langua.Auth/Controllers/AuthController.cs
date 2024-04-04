using Langua.DataContext.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Auth.Controllers
{
    [Route("Controller/action")]
    public partial class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IWebHostEnvironment env;


        SecurityService Security;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IWebHostEnvironment env, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        SecurityService _Security, ILogger<AuthController> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.env = env;
            Security = _Security;
            _logger = logger;
        }

        private IActionResult RedirectWithError(string error, string redirectUrl)
        {
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Redirect($"~/Login?error={error}&redirectUrl={Uri.EscapeDataString(redirectUrl)}");
            }
            else
            {
                return Redirect($"~/Login?error={error}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password, string redirectUrl)
        {


            string userName = Email;


            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(Password))
            {

                //var user = await userManager.FindByNameAsync(userName);

                var result = await signInManager.PasswordSignInAsync(userName, Password, false, false);

                if (result.Succeeded)
                {
                    return Redirect($"~/{redirectUrl}");
                }
            }

            string error = "Invalid user or password";
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Redirect($"~/Login?error={error}&redirectUrl={Uri.EscapeDataString(redirectUrl)}");
            }
            else
            {
                return Redirect($"~/Login?error={error}");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string fullname, string email, string specalite, string phone, string cin, string photo, string password)
        {
            return null;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            return null;
        }


        public async Task<IActionResult> Logout(string RedirectTo = null)
        {
            try
            {
                await Security.Logut();
                return Redirect("/login");
            }
            catch (Exception ex)
            {

            }
            return null;

        }

    }
}
