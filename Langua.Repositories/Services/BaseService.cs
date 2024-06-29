using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langua.DataContext.Data;
using Langua.Models;
using System.Linq.Expressions;
using System.Reflection;
using Langua.Shared.Data;

namespace Langua.Repositories.Services
{
    public class BaseService
    {
        private readonly LanguaContext _context;
        public BaseService(LanguaContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<T>> Apply<T>(IQueryable<T> items, IQueryCollection query = null) where T : class
        {
            if (query is not null && items is not null)
            {
                if (query.ContainsKey("include"))
                {
                    var includes = query["include"].ToString().Split(",");
                    foreach (var include in includes)
                    {
                        items = items.Include(include);
                    }
                }
                var filter = query.ContainsKey("where") ? query["where"].ToString() : null;
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    items = items.Where(filter);
                }

            }
            return await Task.FromResult(items);
        }

        public async Task<T?> GetEntiteByUserId<T>(string userid,Expression<Func<T, bool>> expression) where T : class
        {
            PropertyInfo property = typeof(T).GetProperty("UserId");
            var resultt = _context.Set<T>().AsQueryable();
            var result = _context.Set<T>().AsEnumerable().Where(e=>property.GetValue(e).ToString() == userid).FirstOrDefault();
            return await Task.FromResult(result);
        }

        public Result<IQueryable<T>> GetByExpression<T>(string expression) where T : class
        {
            try
            {

                ParameterExpression paramType = Expression.Parameter(typeof(T), expression);
                ParameterExpression paramExpr = Expression.Parameter(typeof(T));
                var arrProp = expression.Split('.').ToList();
                var result = _context.Set<T>().AsQueryable().Where(expression);


                //var predicate = System.Linq.Dynamic.DynamicExpression.ParseLambda();
                //var result = _context.Set<T>().AsQueryable();
                //result = result.ToList().Where(expressionWithValue);
                return new Result<IQueryable<T>>(true, result);
            }
            catch (Exception ex)
            {
                return new Result<IQueryable<T>>(false, null, ex.Message);
            }
        }
        SemaphoreSlim SemaphoreSlimCount = new SemaphoreSlim(1);
        public async Task<int> NBItems<T>() where T : class
        {
            await SemaphoreSlimCount.WaitAsync();
            var count = await _context.Set<T>().CountAsync();
            SemaphoreSlimCount.Release();
            return await Task.FromResult(count);
        }
        public async Task<int> NBItemsForManager<T>(int ManagerId) where T : class
        {
            await SemaphoreSlimCount.WaitAsync();
            var Manager = _context.Managers.Where(m => m.Id == ManagerId).Include(i => i.Department).FirstOrDefault();
            int count = 0;
            switch (typeof(T).Name)
            {
                case "GroupCandidates":
                    List<int> candidateIds = new List<int>();
                    candidateIds.AddRange(_context.Candidates.Where(p => p.DepartementId== Manager.DepartmentId).Select(i => i.Id).ToList());
                    count = await _context.GroupCandidates.Where(g => candidateIds.Contains(g.CandidatId)).CountAsync();
                    break;
                case "GroupTeacher":
                    List<int> groupIds = new List<int>();
                    groupIds.AddRange(_context.Groups.Where(p => p.DepartmentId == Manager.DepartmentId).Select(i=>i.Id).ToList());
                    count = await _context.GroupTeachers.Where(g=> groupIds.Contains(g.GroupId)).CountAsync();
                    break;
                case "Groups":
                    count = await _context.Groups.Where(p=>p.DepartmentId==Manager.DepartmentId).CountAsync();
                    break;
                case "Department":
                    count = await _context.Departments.Where(p=>p.ManagerId == ManagerId).CountAsync();
                    break;
                case "Teacher":
                    count = await _context.Teachers.Where(t => t.DepartementId == Manager.DepartmentId).CountAsync();
                    break;
                case "Candidat":
                    count = await _context.Candidates.Where(t => t.DepartementId == Manager.DepartmentId).CountAsync();
                    break;
                case "ApplicationUser":
                    List<string> userIds = new List<string>();
                    userIds.AddRange(_context.Candidates.Where(i => i.DepartementId == Manager.DepartmentId).Select(i => i.UserId).ToList());
                    userIds.AddRange(_context.Teachers.Where(i => i.DepartementId == Manager.DepartmentId).Select(i => i.UserId).ToList());
                    count = await _context.Users.Where(i=> userIds.Contains(i.Id)).CountAsync();
                    break;
            }
            SemaphoreSlimCount.Release();
            return await Task.FromResult(count);
            
        }
    }
}
