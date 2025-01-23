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
        public async Task<Result<List<TotalDureeSessions>>> TotalDureeSessions(int? deprt = null)
        {
            try
            {
                if (SecurityService.IsAdmin)
                {
                    List<TotalDureeSessions>? res = await dataAccess.LoadData<TotalDureeSessions>(sp["TotalSesOfGroupInMenute"]!.ToString());
                    return new Result<List<TotalDureeSessions>>(true, res);
                }
                else
                {
                    if (ReferenceEquals(deprt, null))
                    {
                        return new Result<List<TotalDureeSessions>>(
                            false,
                            null!,
                            "The user does not belong to any departement"
                            , new Shared.Exceptions.LanguaException("The user does not belong to any departement"));

                    }
                    else
                    {

                        var res = await dataAccess.LoadData<TotalDureeSessions>(sp["TotalSesOfGroupInMenuteByDep"]!.ToString()!.Replace("@Depart", deprt.ToString()));
                        return new Result<List<TotalDureeSessions>>(true, res);
                    }
                }
            }
            catch (Langua.Shared.Exceptions.LanguaException langex)
            {
                return new Result<List<TotalDureeSessions>>(false, null!, langex.Message);
            }
            catch (Exception ex)
            {
                return new Result<List<TotalDureeSessions>>(false, null!, ex.Message);
            }
        }
        public async Task<Result<List<SessionByGroup>>> NbSessionByGroup(int? deprt = null)
        {
            try
            {
                if (SecurityService.IsAdmin)
                {
                    var res = await dataAccess.LoadData<SessionByGroup>(sp["NbrSessionsForGroup"]!.ToString());
                    return new Result<List<SessionByGroup>>(true, res);
                }
                else
                {
                    if (ReferenceEquals(deprt, null))
                    {
                        return new Result<List<SessionByGroup>>(
                            false,
                            null!,
                            "The user does not belong to any departement"
                            , new Shared.Exceptions.LanguaException("The user does not belong to any departement"));

                    }
                    else
                    {

                        var res = await dataAccess.LoadData<SessionByGroup>(sp["NbrSessionsForGroupByDep"]!.ToString()!.Replace("@Depart", deprt.ToString()));
                        return new Result<List<SessionByGroup>>(true, res);
                    }
                }
            }
            catch (Langua.Shared.Exceptions.LanguaException langex)
            {
                return new Result<List<SessionByGroup>>(false, null!, langex.Message);
            }
            catch (Exception ex)
            {
                return new Result<List<SessionByGroup>>(false, null!, ex.Message);
            }
        }
        public async Task<Result<List<CandidateAlongGroup>>> candidateAlongGroups(int? departementId=null)
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
                    if (ReferenceEquals(null, departementId))
                        return new Result<List<CandidateAlongGroup>>(
                            false,
                            null!,
                            "There is no departement for this user"
                            , new Shared.Exceptions.LanguaException("There is no departement for this user"));

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
