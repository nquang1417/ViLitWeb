using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookReviewsService : IServices<BookReviews>
    {
    }

    public class BookReviewsService : ServiceBase<BookReviews>, IBookReviewsService
    {
        public BookReviewsService(IBookReviewsRepository repository, ViLDbContext dbContext) : base(repository, dbContext) { }

        protected override bool validate(BookReviews entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.GetById(entity.ReviewId);
            if (query != null)
            {
                if (query.BookId != entity.BookId)
                {
                    isValid = false;
                    listErrorMsgs.Add("BookId không được phép thay đổi");
                }
                if (query.UserId != entity.UserId)
                {
                    isValid = false;
                    listErrorMsgs.Add("UserId không được phép thay đổi");
                }
            } else
            {
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
