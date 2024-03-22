using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Infrastructure;
using ViL.Data.Models;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookTagsService : IServices<BookTags>
    {
    }

    public class BookTagsService : ServiceBase<BookTags>, IBookTagsService
    {
        public BookTagsService(IRepository<BookTags> repository, ViLDbContext viLDbContext) : base(repository, viLDbContext)
        {
        }

        protected override bool validate(BookTags entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.Get(bt => bt.BookId == entity.BookId && bt.TagId == entity.TagId).First();
            if (query == null)
            {
                throw new VilNotFoundExceptions("Thông tin không tồn tại");
            }
            return isValid;
        }
    }
}
