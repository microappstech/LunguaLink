using Langua.Api;
using Langua.Api.Shared.ApiHelper;
using Langua.DataContext.Data;
using Langua.ModelView.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Langua.Models;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Langua.Shared.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;


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
        private readonly LanguaContext context;
        private IConfiguration config;
        public AuthController( SecurityService security , SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApiHelper apiHelper, IConfiguration configuration, LanguaContext context)
        {
            _securityService = security;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _apiHelper = apiHelper;
            config= configuration;
            this.context = context;
        }
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            var username = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            await HttpContext.SignOutAsync();
            await signInManager.SignOutAsync();
            //await _securityService.Logut();
            return Redirect("/");
            return Ok("Successfully loged out!");


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
                        new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                    };
                    if (user.FullName is not null)
                        claims.Append(new Claim(ClaimTypes.Name, user.FullName));
                    var token = _apiHelper.GenerateToken(claims);
                    Response.Cookies.Append("token", token);
                    var expireTimeInmunite = int.TryParse(config["AuthSettings:TokenValidityInMinutes"], out int ExpTime) ? ExpTime : 10;
                    return new LoginResponse
                    {
                        Token = token,
                        Success = true,
                        Message = "Successed",
                        UserName = user.UserName,
                        UserId = user.Id,
                        ExpiredAt = DateTime.Now.AddMinutes(expireTimeInmunite),
                    };
                }
                
            }catch(Exception ex)
            {
                return new LoginResponse { Success = false,Message =ex.Message };
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Profile")]
        public async Task<ApiResponse<ResponseProfile>> Profile()
        {
            try
            {
                var ser = User;
                var userid = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email);

                var Item = context.Candidates
                    .Include(i=>i.Group)
                    .Include(i=>i.Departement)
                    .Where(c => c.UserId == userid)
                    .Select(c => new ResponseProfile
                    {
                        FullName = c.FullName,
                        UserId =  c.UserId,
                        Photo = c.Photo,
                        Phone = c.Phone,
                        Email = c.Email,
                        SubjectName = c.Subject!=null? c.Subject.Name:"",
                        CreatedAt = c.CreatedAt,
                        IsConnected = c.IsConnected,
                        GroupId = c.GroupId,
                        DepartementId = c.DepartementId,
                        DepartementName = c.Departement != null ? c.Departement.Name:"",
                        GroupName = c.Group != null ? c.Group.Name : ""
                    })
                    .FirstOrDefault();

                if (Item is null)
                    return new ApiResponse<ResponseProfile> { Success = false, Message = "There No Candidat for this userid" };

                return new ApiResponse<ResponseProfile> { Success = true,  Data = Item};

            }
            catch(Exception ex)
            {
                return new ApiResponse<ResponseProfile> { Success = false, Message = ex.Message };
            }
        }
    }
    
}
