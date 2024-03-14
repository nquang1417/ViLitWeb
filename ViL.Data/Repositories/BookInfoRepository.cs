using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class BookInfoRepository : RepositoryBase<BookInfo>, IBookInfoRepository
    {
       public BookInfoRepository(ViLDbContext context) : base(context) { }
    }

    public interface IBookInfoRepository : IRepository<BookInfo>
    {
    }
}