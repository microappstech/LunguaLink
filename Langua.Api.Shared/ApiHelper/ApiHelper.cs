using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Api.Shared.ApiHelper
{
    public class ApiHelper
    {
        private IConfiguration config;

        public ApiHelper(IConfiguration configuration)
        {
            config = configuration;
        }
        public string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["AuthSettings:Key"].ToString()));
            var r = config["AuthSettings:TokenValidityInMinutes"];
            var rr = config["AuthSetting:TokenValidityInMinutes"];
            var expireTimeInmunite = int.Parse(config["AuthSettings:TokenValidityInMinutes"]);
            var jwt = new JwtSecurityToken(
                issuer: config["AuthSettings:Issuer"],
                audience: config["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireTimeInmunite),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            var tokenAsString = new JwtSecurityTokenHandler().WriteToken(jwt);
            return tokenAsString;
        }

    }
}
