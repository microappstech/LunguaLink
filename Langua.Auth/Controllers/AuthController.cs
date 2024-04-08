using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SecurityService _securityService;
        
        public AuthController( SecurityService security )
        {
            _securityService = security;
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
    }
}
