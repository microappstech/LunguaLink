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
        public async Task<Result<bool>> PublishRessource(ContentGroup CG, List<int> PublishToGroupsId)
        {
            try
            {
                var ExistingItems = Context.GroupRessources.Where(i => PublishToGroupsId.Contains(i.GroupId) && i.RessourceId == CG.RessourceId).AsNoTracking();
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
            if(resResult is not null)
                return await Task.FromResult(new Result<IEnumerable<Ressource>> (true, resResult ));
            return await Task.FromResult(new Result<IEnumerable<Ressource>> (false, null!));

        }
        public async Task<Result<IEnumerable<ContentGroup>>> GetPublishedContentByRessourceId(int RessourceId)
        {
            try
            {
                var Items = Context.GroupRessources.Where(g => g.RessourceId == RessourceId);
                if (Items is not null)
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
