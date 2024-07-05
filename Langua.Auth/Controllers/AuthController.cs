using Langua.Api;
using Langua.Api.Shared.ApiHelper;
using Langua.DataContext.Data;
using Langua.ModelView.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Langua.Models;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Langua.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SecurityService _securityService;
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly ApiHelper _apiHelper;

        public AuthController( SecurityService security , SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApiHelper apiHelper)
        {
            _securityService = security;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _apiHelper = apiHelper;
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _securityService.Logut();
            return Redirect("/login");
        }

        [HttpGet("Test")]
        public async Task Test()
        {

        }
        //public async Task<ApiResponse> Login(string Username, string Password)
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<LoginResponse> Login([FromBody] LoginInput loginInput)
        
        {
            string username = loginInput.Email;
            string password = loginInput.Password;
            try
            {
                ApplicationUser user = await userManager.FindByEmailAsync(username);
                if(user is null) 
                {
                    return new LoginResponse
                    {
                        Success = false, 
                        Message = "No user found" 
                    };
                }
                var signInResult = await signInManager.PasswordSignInAsync(user, password,false,false);
                if(!signInResult.Succeeded)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Password is incorrect"
                    };
                }
                else
                {
                    var claims = new Claim[]
                    {
                        new Claim (ClaimTypes.Email,user.UserName),
                        new Claim (ClaimTypes.NameIdentifier,user.Id),
                    };
                    if (user.FullName is not null)
                        claims.Append(new Claim(ClaimTypes.Name, user.FullName));
                    var token = _apiHelper.GenerateToken(claims);
                    Response.Cookies.Append("token", token);
                    return new LoginResponse
                    {
                        Token = token,
                        Success = true,
                        Message = "Successed"
                    };
                }
                
            }catch(Exception ex)
            {
                return new LoginResponse { Success = false,Message =ex.Message };
            }
        }
    }
}
