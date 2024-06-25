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
    }
}
