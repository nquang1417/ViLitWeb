using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class GenresRepository : RepositoryBase<Genres>, IGenresRepository
    {
       public GenresRepository(ViLDbContext context) : base(context) { }
    }

    public interface IGenresRepository : IRepository<Genres>
    {
    }
}