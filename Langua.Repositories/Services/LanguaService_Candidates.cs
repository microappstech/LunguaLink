using Langua.Account;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.Data.SqlClient;
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
        public Result<IQueryable<Candidat>> GetCandidates(bool isAdmin = false )
        {
            isAdmin = SecurityService.IsAdmin;
            if(isAdmin)
            {
                var Items = Context.Candidates.AsQueryable().AsNoTracking();
                Items = Items.Include(i => i.User).Include(i=>i.Subject);
                if(Items is not null && Items.Any())
                return new Result<IQueryable<Candidat>>(true,Items);
                    return new Result<IQueryable<Candidat>>(false,null!);
            }
            var UserId = new SqlParameter("@UserId", security.User.Id);
            var items = Context.Candidates.FromSqlRaw(
                @"select C.* from Candidates C 
	                    Inner join Departments D on c.DepartementId=d.Id 
	                    Inner Join Managers M on D.Id = M.DepartmentId 
                    where M.UserId = @UserId",UserId);

            //items = items.Include(i => i.Departement).ThenInclude(i=>i.Manager);
            if(items is not null && items.Any())
                return new Result<IQueryable<Candidat>>(true,items);

            return new Result<IQueryable<Candidat>>(false,null!);
        }
    }
}
