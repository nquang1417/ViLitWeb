using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class UserLikedChaptersRepository : RepositoryBase<UserLikedChapters>, IUserLikedChaptersRepository
    {
       public UserLikedChaptersRepository(ViLDbContext context) : base(context) { }
    }

    public interface IUserLikedChaptersRepository : IRepository<UserLikedChapters>
    {
    }
}