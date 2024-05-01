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
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using Microsoft.Net.Http.Headers;



namespace Langua.Api.ApiControllers
{
    [Route("odata/Langua/Groups")]
    public partial class GroupsController : ODataController
    {
        private LanguaContext context;

        public GroupsController(LanguaContext context)
        {
            this.context = context;
        }
        public static IDictionary<string, object> IfMatch(HttpRequest request, Type elementType)
        {
            StringValues ifMatchValues;
            if (request.Headers.TryGetValue("If-Match", out ifMatchValues))
            {
                var etagHeaderValue = EntityTagHeaderValue.Parse(ifMatchValues.SingleOrDefault());
                if (etagHeaderValue != null)
                {
                    var values = request
                        .GetETagHandler()
                        .ParseETag(etagHeaderValue) ?? new Dictionary<string, object>();

                    return elementType
                        .GetProperties()
                        .Where(pi => pi.GetCustomAttributes(typeof(ConcurrencyCheckAttribute), false).Any())
                        .OrderBy(pi => pi.Name)
                        .Select((pi, i) => new { Index = i, Name = pi.Name })
                        .ToDictionary(p => p.Name, p => values[p.Index.ToString()]);
                }
            }

            return null;

        }
        public static bool IsNullable(Type clrType)
        {
            if (clrType.IsValueType)
            {
                return clrType.IsGenericType && clrType.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            else
            {
                return true;
            }
        }

        public static Expression ToNullable(Expression expression)
        {
            if (!IsNullable(expression.Type))
            {
                return Expression.Convert(expression, ToNullable(expression.Type));
            }

            return expression;
        }
        public static Type ToNullable(Type clrType)
        {
            if (IsNullable(clrType))
            {
                return clrType;
            }
            else
            {
                return typeof(Nullable<>).MakeGenericType(clrType);
            }
        }
        public static IQueryable<T> ApplyTo<T>(HttpRequest request, IQueryable<T> query)
        {
            var ifMatch = IfMatch(request, query.ElementType);

            if (ifMatch != null)
            {
                var type = query.ElementType;
                var param = Expression.Parameter(type);
                Expression where = null;
                foreach (var item in ifMatch)
                {
                    var property = query.ElementType.GetProperty(item.Key);
                    var conversionType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var itemValue = (item.Value == null) ? null :
                        Convert.ChangeType(item.Value is DateTimeOffset ?
                            ((DateTimeOffset)item.Value).UtcDateTime : item.Value, conversionType);

                    var name = Expression.Property(param, item.Key);

                    var value = itemValue != null
                        ? IsNullable(property.PropertyType) ? ToNullable(Expression.Constant(itemValue)) : Expression.Constant(itemValue)
                        : Expression.Constant(value: null);

                    var equal = Expression.Equal(name, value);

                    where = where == null ? equal : Expression.AndAlso(where, equal);
                }

                if (where == null)
                {
                    return query;
                }

                return query.Where(Expression.Lambda<Func<T, bool>>(where, param));
            }

            return query;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Groups> GetGroups()
        {
            var items = this.context.Groups.AsQueryable<Groups>();
            items = ApplyTo<Groups>(Request, items);
            this.OnGroupsRead(ref items);

            return items;
        }

        partial void OnGroupsRead(ref IQueryable<Groups> items);

        partial void OnGroupGet(ref SingleResult<Groups> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/LanguaDb/Groups(Id={Id})")]
        public SingleResult<Groups> GetGroup(int key)
        {
            var items = this.context.Groups.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnGroupGet(ref result);

            return result;
        }
        partial void OnGroupDeleted(Groups item);
        partial void OnAfterGroupDeleted(Groups item);

        [HttpDelete("/odata/LanguaDb/Groups(Id={Id})")]
        public IActionResult DeleteGroup(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Groups
                    .Where(i => i.Id == key)
                    .Include(i => i.Candidats)
                    //.Include(i => i.GroupCandidates)
                    //.Include(i => i.GroupTeachers)
                    //.Include(i => i.MessageGroups)
                    //.Include(i => i.Sessions)
                    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Groups>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnGroupDeleted(item);
                this.context.Groups.Remove(item);
                this.context.SaveChanges();
                this.OnAfterGroupDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnGroupUpdated(Groups item);
        partial void OnAfterGroupUpdated(Groups item);

        [HttpPut("/odata/LanguaDb/Groups(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutGroup(int key, [FromBody]Groups item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Groups
                    .Where(i => i.Id == key)
                    .AsQueryable();

                //items = Data.EntityPatch.ApplyTo<Groups>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnGroupUpdated(item);
                this.context.Groups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Groups.Where(i => i.Id == key);
                
                this.OnAfterGroupUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/LanguaDb/Groups(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchGroup(int key, [FromBody]Delta<Groups> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Groups
                    .Where(i => i.Id == key)
                    .AsQueryable();
                //items = Data.EntityPatch.ApplyTo<Groups>(Request, items);
                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnGroupUpdated(item);
                this.context.Groups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Groups.Where(i => i.Id == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnGroupCreated(Groups item);
        partial void OnAfterGroupCreated(Groups item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Groups item)
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

                this.OnGroupCreated(item);
                this.context.Groups.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Groups.Where(i => i.Id == item.Id);

                

                this.OnAfterGroupCreated(item);

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
