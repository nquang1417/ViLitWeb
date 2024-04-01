using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IUserFavoriteBooksService : IServices<UserFavoriteBooks>
    {
    }

    public class UserFavoriteBooksService : ServiceBase<UserFavoriteBooks>, IUserFavoriteBooksService
    {
        public UserFavoriteBooksService(IUserFavoriteBooksRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(UserFavoriteBooks entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.Get(obj => obj.UserId == entity.UserId && obj.BookId == entity.BookId).First();
            if (query == null)
            {
                isValid = false;
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
