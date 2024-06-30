using Azure.Core;
using Langua.Account;
using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Services;
using Langua.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Langua.ApiControllers.ApiControllers
{
    [Route("odata/Langua/Teachers")]
    public partial class TeachersController : ODataController
    {
        private LanguaContext context;
        private SecurityService security;
        private LanguaService languaService;

        public TeachersController(LanguaContext context, SecurityService security, LanguaService languaService)
        {
            this.context = context;
            this.security = security;
            this.languaService = languaService;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IEnumerable<Teacher> GetTeachers()
        {
            var items = this.context.Teachers.AsQueryable<Teacher>();


            if (SecurityService.IsAdmin)
                return items;

            var manager = context.Managers.Where(i => i.UserId == security.User.Id).FirstOrDefault();
            if (manager == null)
                return null;
            
            items = items.Where(i => i.DepartementId == manager.DepartmentId);
            return items;
        }

        partial void OnTeachersRead(ref IQueryable<Teacher> items);

        partial void OnTeacherGet(ref SingleResult<Teacher> item);

        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        [HttpGet("/odata/LanguaDb/Teachers(Id={Id})")]
        public SingleResult<Teacher> GetTeacher(int key)
        {
            var items = this.context.Teachers.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnTeacherGet(ref result);

            return result;
        }
        partial void OnTeacherDeleted(Teacher item);
        partial void OnAfterTeacherDeleted(Teacher item);

        [HttpDelete("/odata/LanguaDb/Teachers(Id={Id})")]
        public IActionResult DeleteTeacher(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Teachers
                    .Where(i => i.Id == key)
                    //.Include(i => i.GroupTeachers)
                    //.Include(i => i.Sessions)
                    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Teacher>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTeacherDeleted(item);
                this.context.Teachers.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTeacherDeleted(item);

                return new NoContentResult();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTeacherUpdated(Teacher item);
        partial void OnAfterTeacherUpdated(Teacher item);

        [HttpPut("/odata/LanguaDb/Teachers(Id={Id})")]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IActionResult PutTeacher(int key, [FromBody] Teacher item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Teachers
                    .Where(i => i.Id == key)
                    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Teacher>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTeacherUpdated(item);
                this.context.Teachers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Teachers.Where(i => i.Id == key);

                this.OnAfterTeacherUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/LanguaDb/Teachers(Id={Id})")]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IActionResult PatchTeacher(int key, [FromBody] Delta<Teacher> patch)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Teachers
                    .Where(i => i.Id == key)
                    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Teacher>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnTeacherUpdated(item);
                this.context.Teachers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Teachers.Where(i => i.Id == key);

                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTeacherCreated(Teacher item);
        partial void OnAfterTeacherCreated(Teacher item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IActionResult Post([FromBody] Teacher item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnTeacherCreated(item);
                this.context.Teachers.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Teachers.Where(i => i.Id == item.Id);



                this.OnAfterTeacherCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }

}
