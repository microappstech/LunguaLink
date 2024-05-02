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

namespace Langua.Api.ApiControllers
{
    [Route("odata/Langua/Sessions")]
    public partial class SessionsController : ControllerBase
    {
        private LanguaContext context;

        public SessionsController(LanguaContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Session> GetSessions()
        {
            var items = this.context.Sessions.AsQueryable<Session>();
            

            return items;
        }

        partial void OnSessionsRead(ref IQueryable<Session> items);

        partial void OnSessionGet(ref SingleResult<Session> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Langua/Sessions(Id={Id})")]
        //[HttpGet("/odata/Langua/Sessions({Id})")]
        public SingleResult<Session> GetSession(int key)
        {
            var items = this.context.Sessions.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnSessionGet(ref result);

            return result;
        }
        partial void OnSessionDeleted(Session item);
        partial void OnAfterSessionDeleted(Session item);

        [HttpDelete("/odata/Langua/Sessions(Id={Id})")]
        public IActionResult DeleteSession(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Sessions
                    .Where(i => i.Id == key)
                    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Session>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSessionDeleted(item);
                this.context.Sessions.Remove(item);
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
        public IActionResult PatchSession(int Id, [FromBody]Delta<Session> patch)
        {
            try
            {
                //Delta<Session> patch = new Delta<Session>();
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
                patch.Patch(item);

                this.OnSessionUpdated(item);
                this.context.Sessions.Update(item);
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
        public IActionResult Post([FromBody] Session item)
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
