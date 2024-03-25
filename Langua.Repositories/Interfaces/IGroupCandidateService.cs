using Langua.Models;
using Langua.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Interfaces
{
    public interface IGroupCandidateService<T> : IRepositoryCrudBase<T> where T : class
    {
        public Result<List<GroupCandidates>> AddCandidateToGroup(Groups group, List<int> candidatsIds);
        public Result<GroupCandidates> CandidateExistInGroup(Groups group, Candidat candidat);
        /// <summary>
        /// Attach Cqndidqte zith one group only one to Many
        /// </summary>
        /// <param name="group"></param>
        /// <param name="candidatsIds"></param>
        /// <returns></returns>
        public Result<GroupCandidates> AddCandidateGroup(Groups group, List<int> candidatsIds);
        public Result<IEnumerable<Candidat>> GetCandidateByGroupId(int groupId);
    }
}
