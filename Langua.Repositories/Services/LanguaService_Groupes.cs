using Langua.Account;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        public async Task<Result<IQueryable<Groups>>> GetGroupes(string includes = "")
        {
            try
            {

                var items = Context.Groups.AsQueryable();
                if(!string.IsNullOrEmpty(includes) )
                    foreach ( var inc in includes.Split(','))
                    {
                        items = items.Include(inc);
                    }
                if (ReferenceEquals(items,null))
                    return await Task.FromResult(new Result<IQueryable<Groups>>(false, null));

                if (SecurityService.IsAdmin)
                    return await Task.FromResult(await Task.FromResult(new Result<IQueryable<Groups>>(true, items)));
                var manager = Context.Managers.Where(i => i.UserId == security.User.Id).FirstOrDefault();
                if (manager == null)
                    return await Task.FromResult(new Result<IQueryable<Groups>>(false, null));

                items = items.Where(i => i.DepartmentId == manager.DepartmentId);
                return await Task.FromResult(await Task.FromResult(new Result<IQueryable<Groups>>(true, items)));


            }
            catch (Exception ex)
            {
                return new Result<IQueryable<Groups>>(false, null,ex.InnerException.Message);
            }
        }
    }
}
