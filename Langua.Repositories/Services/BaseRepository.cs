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
    public class BaseRepositoryCrud<T> :  IRepositoryCrudBase<T> where T : class
    {
        private readonly LanguaContext _context;
        public BaseRepositoryCrud(LanguaContext context)
        {
            _context = context;
        }
        public Result<T> Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
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
                return new Result<IQueryable<T>>(true, result);
            }catch(Exception ex)
            {
                return new Result<IQueryable<T>>(false, null);
            }
        }

        public Result<IQueryable<T>> GetByExpression(string prop, string value)
        {
            PropertyInfo property = typeof(T).GetProperty(prop);
            var result = _context.Set<T>().AsQueryable();
            result = result.Where(x=> property.GetValue(prop).ToString()==value);
            return new Result<IQueryable<T>> (true,result);
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
