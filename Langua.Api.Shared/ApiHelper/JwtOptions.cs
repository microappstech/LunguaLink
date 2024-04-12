using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Api.Shared.ApiHelper
{
    public record class JwtOptions
    {
        public JwtOptions(
            string Issuer,
            string Audience,
            string SigninKey,
            int expiration
        )
        {
                
        }
    }
}
