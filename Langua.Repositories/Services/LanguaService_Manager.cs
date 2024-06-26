using Langua.Models;
using Langua.Shared.Data;
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
        public async Task<Result<Manager>> GetManagerByUserId(string UserId)
        {
            var m = Context.Managers.Where(m=>m.UserId == UserId).FirstOrDefault();
            if(m != null)
                return await Task.FromResult(new Result<Manager>(true,m));

            return await Task.FromResult(new Result<Manager>(false,null!));
        }
        public async Task<Result<IQueryable<Department>>> GetDepartement(bool is_Admin=false, string UserId = "" )
        {
            try
            {
                var items = Context.Departments.AsQueryable();
                if (is_Admin || string.IsNullOrEmpty(UserId))
                    return new Result<IQueryable<Department>>(true, items);

                items = items.Where(d => d.UserId == UserId);
                return new Result<IQueryable<Department>>(true, items);


            }
            catch (Exception ex)
            {
                return new Result<IQueryable<Department>>(false, null,ex.InnerException.Message);
            }
        }
    }
}
