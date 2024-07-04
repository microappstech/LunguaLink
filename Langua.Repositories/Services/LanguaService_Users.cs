using Langua.Account;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        public async Task<Result<IEnumerable<ApplicationUser>>> GetUsers( string includes = "")
        {
            var items = Context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(includes))
                foreach (var inc in includes.Split(","))
                {
                    items = items.Include(inc);
                }
            if (!items.Any())
                return await Task.FromResult(new Result<IEnumerable<ApplicationUser>>(false, null));
            if (SecurityService.IsAdmin)
                return await Task.FromResult(new Result<IEnumerable<ApplicationUser>>(true, items));

            var manager = Context.Managers.Where(i => i.UserId == security.User.Id).FirstOrDefault();
            if (manager == null)
                return await Task.FromResult(new Result<IEnumerable<ApplicationUser>>(false, null));
            
            
            var CandIds = Context.Candidates.Where(i => i.DepartementId == manager.DepartmentId).Select(i => i.UserId).ToList();
            var TeachIds = Context.Teachers.Where(i => i.DepartementId == manager.DepartmentId).Select(i => i.UserId).ToList();
            items = items.Where(i => CandIds.Contains(i.Id) || (TeachIds.Contains(i.Id)==true ));
            return await Task.FromResult(new Result<IEnumerable<ApplicationUser>>(true, items));
        }
    }
}
