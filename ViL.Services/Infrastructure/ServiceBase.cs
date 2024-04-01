using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Services.Infrastructure
{
    public class ServiceBase<T> where T : EntityBase
    {
        protected IRepository<T> _repository;
        protected List<string> listErrorMsgs;
        protected ViLDbContext _context;

        public ServiceBase(IRepository<T> repository, ViLDbContext viLDbContext)
        {
            _repository = repository;
            listErrorMsgs = new List<string>();
            _context = viLDbContext;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _repository.Table;
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> where)
        {
            var query = _repository.Get(where); 
            return query;
        }
        public virtual T GetById(string id)
        {
            var query = _repository.GetById(id);
            return query ?? throw new VilNotFoundExceptions("Not Found!");
        }

        public virtual void Add(T entity)
        {
            if (validate(entity))
            {
                _repository.Add(entity);
            } else
            {
                throw new VilIOExceptions("Dữ liệu không hợp lệ", listErrorMsgs);
            }
        }

        public virtual void Update(T entity)
        {
            if (validate(entity, isUpdate: true))
            {
                _repository.Update(entity);
            } else
            {
                throw new VilIOExceptions("Dữ liệu không hợp lệ", listErrorMsgs);
            }
        }

        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            _repository.Delete(where);
        }
        public virtual void ChangeStatus(T entity, int status)
        {
            if (validate(entity, isUpdate: true))
            {
                entity.Status = status;
                _repository.Update(entity);
            } else
            {
                throw new VilIOExceptions("Dữ liệu không hợp lệ", listErrorMsgs);
            }
        }

        protected virtual bool validate(T entity, bool isUpdate = false)
        {
            var isValid = true;
            if (entity == null)
            {
                listErrorMsgs.Add("Null Reference Exception!");
                return false;
            }
            else
            {
                var properties = entity.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var isrequired = Attribute.IsDefined(property, typeof(RequiredAttribute));
                    if (isrequired)
                    {
                        if (property.GetValue(entity) == null)
                        {
                            isValid = false;
                            listErrorMsgs.Add($"{property.Name} is required");
                        }
                    }
                }
            }
            return isValid;
        }

    }
}
