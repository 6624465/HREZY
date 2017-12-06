using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Web.Services
{
    public interface IRepository<T>
    {
        void Add(T entity);
        //void Update(T entity);
        void Delete(T entity);

        T GetById(int id);

        IEnumerable<T> GetAll();
    }
}
