using Langua.DataContext.Data;
using Langua.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : class
    {
        private readonly LanguaContext _context;
        public BaseRepository(LanguaContext context)
        {
            this._context = context;
        }
        T IRepositoryBase<T>.Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch
            {
                throw new Exception("Creation Failed");
            }
        }

        bool IRepositoryBase<T>.Delete(T entity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<T> IRepositoryBase<T>.GetAll()
        {
            throw new NotImplementedException();
        }

        T IRepositoryBase<T>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        bool IRepositoryBase<T>.Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
