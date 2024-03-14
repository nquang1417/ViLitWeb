using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class BookTagsRepository : RepositoryBase<BookTags>, IBookTagsRepository
    {
       public BookTagsRepository(ViLDbContext context) : base(context) { }
    }

    public interface IBookTagsRepository : IRepository<BookTags>
    {
    }
}