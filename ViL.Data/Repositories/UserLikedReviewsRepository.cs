using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class UserLikedReviewsRepository : RepositoryBase<UserLikedReviews>, IUserLikedReviewsRepository
    {
       public UserLikedReviewsRepository(ViLDbContext context) : base(context) { }
    }

    public interface IUserLikedReviewsRepository : IRepository<UserLikedReviews>
    {
    }
}