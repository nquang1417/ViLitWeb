using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class BookmarksRepository : RepositoryBase<Bookmarks>, IBookmarksRepository
    {
       public BookmarksRepository(ViLDbContext context) : base(context) { }
    }

    public interface IBookmarksRepository : IRepository<Bookmarks>
    {
    }
}