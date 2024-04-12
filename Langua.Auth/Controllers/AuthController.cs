using Langua.Api;
using Langua.Api.Shared.ApiHelper;
using Langua.DataContext.Data;
using Microsoft.AspNetCore.Identity;
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
        [HttpPost("Login")]
        public async Task<ApiResponse> Login(string Username, string Password)
        {
            string username = Username;
            string password = Password;
            try
            {
                var user = await userManager.FindByEmailAsync(username);
                if(user is null) 
                {
                    return new ApiResponse 
                    {
                        Success = false, 
                        Message = "No user found" 
                    };
                }
                var signInResult = await signInManager.PasswordSignInAsync(user, password,false,false);
                if(!signInResult.Succeeded)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Password is incorrect"
                    };
                }
                else
                {
                    var claims = new Claim[]
                    {
                        new Claim ("sub",user.UserName),
                        new Claim ("given_name",user.UserName.Substring(0,user.UserName.IndexOf("@"))),
                        new Claim ("EmailAddress",user.Email)
                    };
                    var token = _apiHelper.GenerateToken(claims);
                    Response.Cookies.Append("token", token);
                    return new ApiResponse<object>
                    {
                        Data = new
                        {
                            token
                        },
                        Success = true,
                        Message = ""
                    };
                }
                
            }catch(Exception ex)
            {
                return new ApiResponse { Success = false,Message =ex.Message };
            }
        }
    }
}
