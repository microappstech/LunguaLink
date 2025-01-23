using Langua.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langua.DataContext.Data;
using Langua.Shared.Exceptions;

namespace Langua.Core.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly LanguaContext _context;
        public BaseRepository(LanguaContext languaContext)
        {
            this._context = languaContext !;
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
                throw new LanguaException();
            }

        }

        public bool Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new LanguaException();
            }

        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw new LanguaException();
            }
        }

        public T GetById(int id)
        {
            try
            {
                return _context.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                throw new LanguaException();
            }
        }

        public bool Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new LanguaException();
            }
        }
    }
}
