using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using Langua.Account;
//using Langua.DataContext.Data;
//using Langua.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Langua.Api.Shared.ApiHelper
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration config;
        public JwtMiddleware(RequestDelegate nextRequest,IConfiguration configuration)
        {
            _next = nextRequest;
            config = configuration;
        }
        //public async Task Idnvoke(HttpContext context,SecurityService security)
        //{
        //    var token = context.Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last();
        //    if (token is not null)
        //    {
        //        await AttachUserToContext(context, security, token);
        //    }
        //    await _next(context);
        //}
        //public async Task AttachUserToContext(HttpContext context, SecurityService security, string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var key = Encoding.ASCII.GetBytes(config["AuthSetting::Secret"]);
        //        tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
                    
        //            // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //            ClockSkew = TimeSpan.Zero
        //        },out SecurityToken validatedToken);
        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id").Value;
        //        context.Items["User"] = await security.GetById(userId);

        //    }
        //    catch(Exception ec)
        //    {
        //    }
        //}
        //public string GenerateAccessToken(ApplicationUser user)
        //{
        //    var claims = new ClaimsIdentity (new[]
        //    {
        //        new Claim("Id",user.Id),
        //        new Claim("UserName", user.UserName),
        //    });
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(config["AuthSettings:Key"].ToString());
        //    var TokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = claims,
        //        Issuer = config["AuthSettings:Issuer"],
        //        Expires = DateTime.Now.AddMinutes(int.Parse(config["AuthSettings:TokenValidityInMinutes"])),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(TokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            return "";
        }
    }
}
