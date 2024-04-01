using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IUserLikedReviewsService : IServices<UserLikedReviews>
    {
    }

    public class UserLikedReviewsService : ServiceBase<UserLikedReviews>, IUserLikedReviewsService
    {
        public UserLikedReviewsService(IUserLikedReviewsRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(UserLikedReviews entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.Get(obj => obj.UserId == entity.UserId && obj.ReviewId == entity.ReviewId).First();
            if (query == null)
            {
                isValid = false;
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
