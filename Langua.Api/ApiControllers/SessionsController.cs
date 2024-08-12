using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Routing.Matching;
using Langua.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Langua.Api.ApiControllers
{
    [Route("odata/Langua/Sessions")]
    [Authorize]
    //[ApiController]
    public partial class SessionsController : ControllerBase
    {
        private LanguaContext context;
        private IMailService mailService;
        private LanguaService languaService;
        private SecurityService _security;
        public SessionsController(LanguaContext context,IMailService mail,LanguaService languaService, SecurityService securityService)
        {
            this.context = context;
            this.mailService = mail;
            this.languaService = languaService;
            _security = securityService;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Session> GetSessions()
        {
            var items = this.context.Sessions.AsQueryable<Session>();          
            if(SecurityService.IsAdmin)
                return items;
            var manager = context.Managers.Where(i => i.UserId == _security.User.Id).FirstOrDefault();
            if (manager is null)
                return null;
            var groupIds = context.Groups.Where(g=>g.DepartmentId == manager.DepartmentId).Select(i=>i.Id).ToList();
            var teacherIds = context.Teachers.Where(g=>g.DepartementId== manager.DepartmentId).Select(i => i.Id).ToList();
            if(groupIds is not null)
                items = items.Where(i => groupIds.Contains(i.GroupId));
            if(teacherIds is not null)
                items = items.Where(i => teacherIds.Contains(i.TeacherId));
            return items;

        }

        [HttpGet("SessionForGroup")]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ApiResponse<List<SessionResponse>> GetSessionForGroup(int groupId)
        {
            try
            {
                var result = context.Sessions.Where(i=>i.GroupId == groupId).Include(i=>i.Teacher).ToList();
                var data = result.Select(i => new SessionResponse
                {
                    Id = i.Id,
                    End=i.End,
                    Start=i.Start,
                    TeacherName=i.Teacher.FullName,
                    TeacherId=i.Teacher.Id,


                }).ToList();
                return new ApiResponse<List<SessionResponse>>(true, data);
            }catch(Exception ex)
            {
                return new ApiResponse<List<SessionResponse>>(false, null!, ex.Message);
            }
        }
        partial void OnSessionsRead(ref IQueryable<Session> items);

        partial void OnSessionGet(ref SingleResult<Session> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Langua/Sessions(Id={Id})")]
        public Session GetSession(int Id)
        {
            var items = this.context.Sessions.Where(i => i.Id == Id).Include(s=>s.Group).ThenInclude(s=>s.Candidats).FirstOrDefault();
            
            //var result = SingleResult.Create(items);

            //OnSessionGet(ref result);

            return items;
        }
        partial void OnSessionDeleted(Session item);
        partial void OnAfterSessionDeleted(Session item);

        [HttpDelete("/odata/Langua/Sessions(Id={Id})")]
        public IActionResult DeleteSession(int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Sessions
                    .Where(i => i.Id == Id)
                    .AsQueryable();



                //items = Data.EntityPatch.ApplyTo<Session>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSessionDeleted(item);
                this.context.Sessions.Remove(item);
                var Candidates = this.context.Candidates.Where(c => c.GroupId == item.GroupId);
                Dictionary<string, string> candidatesEmail = new();
                foreach (var cand in Candidates)
                {
                    candidatesEmail.Add(cand.FullName, cand!.Email);
                }
                var SendMailToCands = mailService.SendMails("Session canceled", @$"Admin langua annulled session <h3>{item.Name}</h3> ", candidatesEmail);
                this.context.SaveChanges();
                this.OnAfterSessionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSessionUpdated(Session item);
        partial void OnAfterSessionUpdated(Session item);

        [HttpPut("/odata/Langua/Sessions(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSession(int key, [FromBody]Session item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Sessions
                    .Where(i => i.Id == key)
                    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Session>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSessionUpdated(item);
                this.context.Sessions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sessions.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Group,Teacher");
                this.OnAfterSessionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost("/odata/Langua/Session/Update")]
        [HttpPatch("/odata/Langua/Update")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSession(int Id, [FromBody]Session patch)
        {
            try
            {
                //Delta<Session> patch = new Delta<Session>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //var items = this.context.Sessions
                //    .Where(i => i.Id == Id)
                //    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Session>(Request, items);

                //var item = items.FirstOrDefault();

                //if (item == null)
                //{
                //    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                //}
                //patch.Patch(item);
                
                this.context.Sessions.Update(patch);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sessions.Where(i => i.Id == Id);
                Request.QueryString = Request.QueryString.Add("$expand", "Group,Teacher");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSessionCreated(Session item);
        partial void OnAfterSessionCreated(Session item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public async Task<IActionResult> Post([FromBody] Session item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnSessionCreated(item);
                var Candidates = context.Candidates.Where(c => c.GroupId == item.GroupId);
                
                Dictionary<string,string> candidatesEmail = new ();
                foreach (var cand in Candidates)
                {
                    candidatesEmail.Add(cand.FullName, cand!.Email);
                }
                var SendMailToCands = await mailService.SendMails("New Session Created", @$"Admin langua create a new session <h3>{item.Name}</h3> in {item.Start.ToShortDateString()} from {item.Start.TimeOfDay } to {item.End.TimeOfDay}
                            . <br/> Don't forget to present to the session", candidatesEmail); 
                if(SendMailToCands)
                    this.context.Sessions.Add(item);
                
                this.context.SaveChanges();
                var itemToReturn = this.context.Sessions.Where(i => i.Id == item.Id);
                Request.QueryString = Request.QueryString.Add("$expand", "Group,Teacher");

                this.OnAfterSessionCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
