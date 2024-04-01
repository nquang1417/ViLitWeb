using System.Linq.Expressions;
using ViL.Data.Models;

namespace ViL.Services.Infrastructure
{
    public interface IServices<T> where T : EntityBase
    {
        IQueryable<T> GetAll();   
        IQueryable<T> Get(Expression<Func<T, bool>> where);
        T GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void ChangeStatus(T entity, int status);
    }
}
