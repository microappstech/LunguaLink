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
    using static Langua.DAL.Sp.SqlProcedure;

namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        public async Task<Result<List<CandidateAlongGroup>>> candidateAlongGroups(int departementId)
        {
            try
            {
                if (SecurityService.IsAdmin)
                {
                    var res = await dataAccess.LoadData<CandidateAlongGroup>(sp["CandidateAlongGroup"]!.ToString());
                    return new Result<List<CandidateAlongGroup>>(true, res);
                }
                else
                {
                    var res = await dataAccess.LoadData<CandidateAlongGroup>(sp["CandidateAlongGroup"]!.ToString()!.Replace("@Depart", departementId.ToString()));
                    return new Result<List<CandidateAlongGroup>>(true, res);
                }

            }catch(Exception ex)
            {
                return new Result<List<CandidateAlongGroup>>(false, null, ex.Message, null, ex);
            }
        }
        public Result<IQueryable<Candidat>> GetCandidates(string includes ="")
        {
            if (SecurityService.IsAdmin)
            {
                var Items = Context.Candidates.AsQueryable();
                if(!string.IsNullOrEmpty(includes))
                    foreach(var inc in includes.Split(","))
                    {
                        Items = Items.Include(inc);
                    } 
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

            if (!string.IsNullOrEmpty(includes))
                foreach (var inc in includes.Split(","))
                {
                    items = items.Include(inc);
                }
            if (items is not null && items.Any())
                return new Result<IQueryable<Candidat>>(true,items);

            return new Result<IQueryable<Candidat>>(false,null!);
        }
    }
}
