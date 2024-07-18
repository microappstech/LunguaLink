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


namespace Langua.Account.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _securityService.Logut();
            return Ok("Successfully loged out!");

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
        [HttpGet("Profile")]
        public async Task<ApiResponse<ResponseProfile>> Profile()
        {
            try
            {
                var ser = User;
                var userid = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email);

                SqlParameter UserId = new SqlParameter("@UserId", "e710ea0-7a07-43d1-87a5-290ca4413a72");

                //var Items = context.from .FromSqlRaw(
                //    @"  
                //        select 
                //         FullName,
                //         Cn.UserId, 
                //         Cn.Photo, 
                //         Cn.Phone, 
                //         Cn.Email,
                //         Sb.Name as SubjectName,
                //         Cn.CreatedAt,
                //         Cn.IsConnected,
                //         Dep.Name as DepartementName,
                //         Gr.Name as GroupName 
                //        from Candidates Cn
                //        left join Departments Dep
                //         on Cn.DepartementId = Dep.Id
                //        left Join Subjects Sb 
                //         on Sb.Id = Cn.SubjectId
                //        Left join Groups Gr 
                //         on Cn.GroupId = Gr.Id
                //        where Cn.UserId = @UserId", UserId);

                //var Item = Items.FirstOrDefault();
                var Item = context.Candidates
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
    public class ResponseProfile
    {
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        public string? Photo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? SubjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsConnected { get; set; }
        public string? DepartementName { get; set; }
        public string? GroupName { get; set; }
        public bool ConfirmedMail { get; set; }

    }
}
