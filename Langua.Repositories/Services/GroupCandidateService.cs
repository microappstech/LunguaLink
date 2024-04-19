using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public class GroupCandidateService<T> : IGroupCandidateService<T> where T : GroupCandidates
    {
        private readonly LanguaContext context;
        public GroupCandidateService(LanguaContext context) 
        { 
            this.context = context;
        }
        public Result<T> Add(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
                return new Result<T>(true,entity);
            }catch(Exception ex){
                return new Result<T>(false,Error:ex.Message,exception:ex);
            }
        }

        public Result<List<GroupCandidates>> AddCandidateToGroup(Groups group, List<int> candidatsIds)
        {
            try
            {
                var GrCands = new List<GroupCandidates>() { };
                foreach (var candidatId in candidatsIds)
                {
                    var GrCand = new GroupCandidates() { CandidateId = candidatId, GroupId = group.Id };
                    context.GroupCandidates.Add(GrCand);
                    GrCands.Add(GrCand);
                }
                context.SaveChanges ();
                return new Result<List<GroupCandidates>>(true,Data:GrCands);
            }
            catch(Exception e)
            {
                return new Result<List<GroupCandidates>>(false,exception:e);
            }
        }

        public Result<GroupCandidates> CandidateExistInGroup(Groups group, Candidat candidat)
        {
            try
            {
                var IsExist = context.GroupCandidates.Where(i=>i.GroupId==group.Id && i.CandidateId == candidat.Id).Any();
                if (IsExist)
                {
                    return new Result<GroupCandidates>(true);
                }
                else 
                {
                    return new Result<GroupCandidates>(false); 
                }
            }catch(Exception e )
            {
                return new Result<GroupCandidates>(false, exception:e);
            }
        }

        public Result<T> Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
                return new Result<T>(true);
            }
            catch (Exception ex)
            {
                return new Result<T>(false, Error: ex.Message);
            }
        }

        public Result<IQueryable<T>> GetAll()
        {
            try
            {
                IQueryable<T> result = context.Set<T>().AsQueryable();
                return new Result<IQueryable<T>>(true, result);
            }
            catch (Exception ex)
            {
                return new Result<IQueryable<T>>(false, null);
            }
        }

        public Result<T> GetById(int id)
        {
            T result = context.Set<T>().Find(id);
            return new Result<T>(true, result);
        }

        public Result<T> Update(T entity)
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
                return new Result<T>(true, entity);
            }
            catch (Exception ex)
            {
                return new Result<T>(true, entity, Error: ex.Message); ;
            }
        }
        public Result<GroupCandidates> AddCandidateGroup(Groups group, List<int> candidatsIds)
        {
            try
            {
                foreach(var candidateId in candidatsIds)
                {
                    var candidate = context.Candidates.Where(i => i.Id == candidateId).FirstOrDefault();
                    if(candidate != null)
                    {
                        candidate.GroupId = group.Id;
                        context.Candidates.Update(candidate);
                    }
                }
                context.SaveChanges();
                return new Result<GroupCandidates>(true);
            }catch(Exception e)
            {
                return new Result<GroupCandidates>(false,exception:e);
            }
        }
        public Result<IQueryable<Candidat>> GetCandidateByGroupId(int groupId)
        {

            try
            {
                var Candidates = context.Candidates.Where(i => i.GroupId == groupId).AsQueryable();
                return new Result<IQueryable<Candidat>>(true,Data:Candidates);
            }
            catch (Exception e)
            {
                return new Result<IQueryable<Candidat>>(false, exception: e);
            }
        }

        public Result<IQueryable<T>> GetByExpression(string expressionWithValue)
        {
            throw new NotImplementedException();
        }
    }
}
