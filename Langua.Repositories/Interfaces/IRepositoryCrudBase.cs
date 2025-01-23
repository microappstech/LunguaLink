using Langua.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Interfaces
{
    public interface IRepositoryCrudBase<T> where T : class
    {
        Result<T> GetById(int id);
        Result<T> Add(T entity);
        Result<T> Update(T entity);
        Result<T> Delete(T entity);
        Result<T> Delete(T entity, string[] inc);
        Result<IQueryable<T>> GetAll();
        Result<IQueryable<T>> GetByExpression(string expressionWithValue);
        //public List<T> GetList(Expression<Func<T, bool>> predicate);

    }
}
