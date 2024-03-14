using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class UserFavoriteBooksRepository : RepositoryBase<UserFavoriteBooks>, IUserFavoriteBooksRepository
    {
       public UserFavoriteBooksRepository(ViLDbContext context) : base(context) { }
    }

    public interface IUserFavoriteBooksRepository : IRepository<UserFavoriteBooks>
    {
    }
}