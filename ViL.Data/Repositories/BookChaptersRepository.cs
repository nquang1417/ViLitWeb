using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class BookChaptersRepository : RepositoryBase<BookChapters>, IBookChaptersRepository
    {
       public BookChaptersRepository(ViLDbContext context) : base(context) { }
    }

    public interface IBookChaptersRepository : IRepository<BookChapters>
    {
    }
}