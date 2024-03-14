using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class ReadingHistoryRepository : RepositoryBase<ReadingHistory>, IReadingHistoryRepository
    {
       public ReadingHistoryRepository(ViLDbContext context) : base(context) { }
    }

    public interface IReadingHistoryRepository : IRepository<ReadingHistory>
    {
    }
}