using System.Linq.Expressions;
using ViL.Data.Models;

namespace ViL.Data.Infrastructure
{
    public interface IRepository<T> where T : EntityBase
    {
        IQueryable<T> Table { get; }
        void Add (T entity);
        void Update (T entity);
        void Delete (T entity);
        void Delete (Expression<Func<T, bool>> where);
        T? GetById(string id);
        IQueryable<T> Get(Expression<Func<T, bool>> where);
    }
}
