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
        public async Task<Result<bool>> PublishRessource(int ressourceId, List<int> PublishToGroupsId)
        {
            try
            {
                PublishToGroupsId.ForEach(GroupId =>
                {
                    ContentGroup CG = new ContentGroup();
                    CG.GroupId = GroupId;
                    CG.RessourceId = ressourceId;
                    Context.GroupRessources.Add(CG);
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
            return await Task.FromResult(new Result<IEnumerable<Ressource>> (false, null));

        }

    }
}
