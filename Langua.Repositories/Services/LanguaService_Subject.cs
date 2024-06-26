using Langua.Account;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        public Result<IQueryable<Subject>> GetSubjects()
        {
            bool isAdmin = SecurityService.IsAdmin;
            if (isAdmin)
            {
                var Items = Context.Subjects.AsQueryable().AsNoTracking();
                if (Items is not null && Items.Any())
                    return new Result<IQueryable<Subject>>(true, Items);
                return new Result<IQueryable<Subject>>(false, null!);
            }
            var UserId = new SqlParameter("@UserId", security.User.Id);
            var items = Context.Subjects.FromSqlRaw(@"select * from Subjects Where UserId = @UserId", UserId);

            //items = items.Include(i => i.Departement).ThenInclude(i=>i.Manager);
            if (items is not null && items.Any())
                return new Result<IQueryable<Subject>>(true, items);

            return new Result<IQueryable<Subject>>(false, null!);
        }

    }
}
