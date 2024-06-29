
using System.Linq.Dynamic.Core;
using Langua.DataContext.Data;
using Langua.Repositories.Interfaces;
using Langua.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public class BaseRepositoryCrud<T> : IRepositoryCrudBase<T> where T : class
    {
        private readonly LanguaContext _context;
        public BaseRepositoryCrud(LanguaContext context)
        {
            _context = context;
        }
        public void Reload() => _context.ChangeTracker.Entries().ToList().ForEach(e => {
            if (e is not null)
                e.State = EntityState.Detached;
        });
        public void Reset() => _context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
        public Result<T> Add(T entity)
        {
            try
            {
                Reset();
                _context.Add(entity);
                _context.SaveChanges();
                return new Result<T>(true,entity);
            }
            catch
            {
                throw new Exception("Creation Failed");
            }
        }

        public Result<T> Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
                return new Result<T>(true);
            }
            catch(Exception ex)
            {
                return new Result<T>(false,Error:ex.Message);
            }
        }

        public Result<IQueryable<T>> GetAll()
        {
            try
            {
                IQueryable<T> result = _context.Set<T>().AsQueryable();
                if (result is not null && result.Any())
                    return new Result<IQueryable<T>>(true, result);
                return new Result<IQueryable<T>>(false,null);
            }catch(Exception ex)
            {
                return new Result<IQueryable<T>>(false, null, Error:ex.Message);
            }
        }

        public Result<IQueryable<T>> GetByExpression(string expression)
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
            }catch(Exception ex)
            {
                return new Result<IQueryable<T>>(false, null,ex.Message);
            }
        }

        public Result<T> GetById(int id)
        {
            T result = _context.Set<T>().Find(id);
            return new Result<T>(true, result);
        }

        public Result<T> Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return new Result<T>(true, entity);
            }
            catch(Exception ex)
            {
                return new Result<T>(true, entity, Error: ex.Message); ;
            }
        }
    }
}
