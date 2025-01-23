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
        public Task<Result<IQueryable<Candidat>>> GetCandidateForGroups(List<int> GroupTeacherIds)
        {
            var items = this.Context.Candidates.Where(i => GroupTeacherIds.Contains((int)i.GroupId)).AsQueryable();

            if(items.Any())
                return Task.FromResult(new Result<IQueryable<Candidat>>(true, items));
            return Task.FromResult(new Result<IQueryable<Candidat>>(false, null, "No Candidat Exist"));
        }

    }
}
