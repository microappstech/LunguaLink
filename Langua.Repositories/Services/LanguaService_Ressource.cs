using Langua.Account;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        public async Task<Result<IEnumerable<Ressource>>> GetRessources(string includes ="")
        {
            var resResult = Context.Ressources
                .Include(g => g.GroupRessources)
                .ThenInclude(i => i.Group).AsQueryable();
            if(resResult is null )
                return await Task.FromResult(new Result<IEnumerable<Ressource>>(false, null!));
            if(!string.IsNullOrEmpty(includes))
                foreach(var inc in includes.Split(','))
                {
                    resResult = resResult.Include(inc);
                }
            if(SecurityService.IsAdmin)
                return await Task.FromResult(new Result<IEnumerable<Ressource>>(true, resResult));
            var Manager = Context.Managers.Where(i => i.UserId == security.User.Id).FirstOrDefault();
            if(Manager == null )
                return await Task.FromResult(new Result<IEnumerable<Ressource>>(false, null!));
            var teacherIds = Context.Teachers.Where(t=>t.DepartementId==Manager.DepartmentId).Select(i=>i.Id).ToList();
            if(teacherIds is not null)
                resResult = resResult.Where(i => teacherIds.Contains((int)i.TeacherId) == true);

            resResult = resResult.AsNoTracking();            
            return await Task.FromResult(new Result<IEnumerable<Ressource>>(true, resResult));
            

        }

        public async Task<Result<bool>> PublishRessource(ContentGroup CG, List<int> PublishToGroupsId)
        {
            try
            {
                var ExistingItems = Context.GroupRessources.Where(i => i.RessourceId == CG.RessourceId).AsNoTracking();
                Context.ChangeTracker.Entries().Where(etity => etity.State== EntityState.Modified).ToList().ForEach(ent => {
                    ent.State = EntityState.Unchanged;
                });
                Reset();
                if(ExistingItems.Any())
                    Context.GroupRessources.RemoveRange(ExistingItems);

                PublishToGroupsId.ForEach(GroupId =>
                {
                    ContentGroup CGP = new ContentGroup();
                    CGP.HideOn = CG.HideOn;
                    CGP.GroupId = GroupId;
                    CGP .RessourceId = CG.RessourceId;
                    Context.GroupRessources.Add(CGP);
                });

                await Context.SaveChangesAsync();
                return await Task.FromResult(new Result<bool>(true, true));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new Result<bool>(false,false,ex.Message));
            }
        }
        public async Task<Result<IEnumerable<Ressource>>> GetRessourceByTeacherId (int TeacherId)
        {
            var resResult = Context.Ressources.Where(r => r.TeacherId == TeacherId).AsQueryable();
            resResult = resResult.Include(i => i.GroupRessources).ThenInclude(i => i.Group);
            resResult = resResult.AsNoTracking();
            if(resResult is not null)
                return await Task.FromResult(new Result<IEnumerable<Ressource>> (true, resResult ));
            return await Task.FromResult(new Result<IEnumerable<Ressource>> (false, null!));

        }
        public async Task<Result<IEnumerable<ContentGroup>>> GetPublishedContentByRessourceId(int RessourceId)
        {
            try
            {
                var Items = Context.GroupRessources.Where(g => g.RessourceId == RessourceId);
                if (Items is not null && Items.Count()>0)
                    return await Task.FromResult(new Result<IEnumerable<ContentGroup>>(true, Items));
                return await Task.FromResult(new Result<IEnumerable<ContentGroup>>(false, null!,"There is no data"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new Result<IEnumerable<ContentGroup>>(false, null!, ex.Message));
            }
        }

    }
}
