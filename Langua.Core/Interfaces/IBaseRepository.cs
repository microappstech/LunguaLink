using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Core.Interfaces
{
    public interface IBaseRepository<T>  where T : class
    {
        T GetById(int id);
        T Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        IEnumerable<T> GetAll();
    }
}
