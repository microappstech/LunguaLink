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
        public T Add(T entity)
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

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
