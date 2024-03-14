using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class UserLikedCommentsRepository : RepositoryBase<UserLikedComments>, IUserLikedCommentsRepository
    {
       public UserLikedCommentsRepository(ViLDbContext context) : base(context) { }
    }

    public interface IUserLikedCommentsRepository : IRepository<UserLikedComments>
    {
    }
}