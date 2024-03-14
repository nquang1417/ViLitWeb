using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class UsersRepository : RepositoryBase<Users>, IUsersRepository
    {
       public UsersRepository(ViLDbContext context) : base(context) { }
    }

    public interface IUsersRepository : IRepository<Users>
    {
    }
}