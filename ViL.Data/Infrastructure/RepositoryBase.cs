using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViL.Data.Models;

namespace ViL.Data.Infrastructure
{
    public class RepositoryBase<T> where T : EntityBase
    {
        private ViLDbContext _context;
        private DbSet<T> dbset;

        public RepositoryBase(ViLDbContext context)
        {
            _context = context;
            this.dbset = _context.Set<T>();
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.dbset;
            }
        }

        public virtual void Add(T entity)
        {
            
            dbset.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            dbset.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
            _context.SaveChanges();

        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach(T entity in objects)
            {
                dbset.Remove(entity);
            }
            _context.SaveChanges();
        }

        public virtual T? GetById(string id)
        {
            return dbset.Find(id);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where<T>(where).AsEnumerable();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
