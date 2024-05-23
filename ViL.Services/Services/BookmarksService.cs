using System.Linq.Expressions;
using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookmarksService : IServices<Bookmarks>
    { 
    }

    public class BookmarksService : ServiceBase<Bookmarks>, IBookmarksService
    {
        private IBookChaptersRepository _chaptersRepository;

        public BookmarksService(IBookmarksRepository repository,
                                IBookChaptersRepository chaptersRepository,
                                ViLDbContext dbContext) : base(repository, dbContext)
        {
            _chaptersRepository = chaptersRepository;
        }

        /*public override IQueryable<Bookmarks> Get(Expression<Func<Bookmarks, bool>> where)
        {
            var bookmarks = _repository.Get(where);
        }*/

        protected override bool validate(Bookmarks entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity);
            }
            var isValid = true;
            var query = _repository.GetById(entity.BookmarkId);
            if (query != null)
            {
                if (query.UserId != entity.UserId)
                {
                    isValid = false;
                    listErrorMsgs.Add("UserId không được phép thay đổi");
                }
                if (query.ChapterId != entity.ChapterId)
                {
                    isValid = false;
                    listErrorMsgs.Add("ChapterId không được phép thay đổi");
                }
            } else
            {
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }

}
