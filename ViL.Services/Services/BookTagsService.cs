using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookTagsService : IServices<BookTags>
    {
    }

    public class BookTagsService : ServiceBase<BookTags>, IBookTagsService
    {
        public BookTagsService(IBookTagsRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
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
