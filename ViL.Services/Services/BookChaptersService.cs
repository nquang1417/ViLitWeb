using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookChaptersService : IServices<BookChapters>
    {
    }

    public class BookChaptersService : ServiceBase<BookChapters>, IBookChaptersService
    {

        public BookChaptersService(IBookChaptersRepository chapterRepository, ViLDbContext dbContext) : base(chapterRepository, dbContext) 
        {
        }

        protected override bool validate(BookChapters entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity);
            }
            var isValid = true;
            var query = _repository.GetById(entity.ChapterId);
            if (query != null)
            {
                if (query.BookId != entity.BookId)
                {
                    isValid = false;
                    listErrorMsgs.Add("BookId không được phép thay đổi");
                }
            } else
            {
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
