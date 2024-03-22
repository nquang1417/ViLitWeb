using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViL.Data.Models;

namespace ViL.Services.Infrastructure
{
    public interface IServices<T> where T : EntityBase
    {
        IEnumerable<T> GetAll();   
        IEnumerable<T> Get(Expression<Func<T, bool>> where);
        T GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void ChangeStatus(T entity, int status);
    }
}
