using Dapper;
using Langua.Account;
using Langua.Api.Shared.ApiHelper;
using Langua.DAL;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using static Langua.DAL.Sp.SqlProcedure;

namespace Langua.Api.ApiControllers
{
    [Route("api/Global")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class GCandidateController:ControllerBase
    {
        private readonly SecurityService Security;
        private ILogger<GCandidateController> logger;
        private readonly IRepositoryCrudBase<Groups> GroupService;
        private readonly ISqlDataAccess dataAccess;
        private readonly ApiHelper _apiHelper;

        public GCandidateController(SecurityService security, ILogger<GCandidateController> logger, IRepositoryCrudBase<Groups> GService,
            ISqlDataAccess dataAccess, ApiHelper apiHelper)
        {
            Security = security;
            this.logger = logger;
            this.GroupService = GService;
            this.dataAccess = dataAccess;
            _apiHelper = apiHelper;
        }
        [HttpGet("GetGroup")]
        public async Task<IActionResult> GetGroup()
        {
            try
            {
                var user = ((ClaimsIdentity)User.Identity);
                var userName = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email);
                var paramss = new DynamicParameters();
                paramss.Add("@UserId", "e8935b0d-5c91-40db-8b68-b93688e2fd31");
                var Re = dataAccess.LoadData<Groups, DynamicParameters>(sp["LoadCandidateGroup"].ToString()!, paramss);
                return Ok(Re.Result);
            }
            catch(Exception ex)
            {
                return BadRequest($"BadRequest GetGroup : {ex.Message} ");
            }
        }
    }
}
