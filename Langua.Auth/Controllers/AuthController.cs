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
        [HttpPost("Logout")]
        public async Task Logout()
        {
            Console.WriteLine("Loogin out");
        }

        [HttpGet("Test")]
        public async Task Test()
        {

        }
    }
}
