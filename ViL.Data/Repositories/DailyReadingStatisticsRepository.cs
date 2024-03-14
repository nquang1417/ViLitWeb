using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class DailyReadingStatisticsRepository : RepositoryBase<DailyReadingStatistics>, IDailyReadingStatisticsRepository
    {
       public DailyReadingStatisticsRepository(ViLDbContext context) : base(context) { }
    }

    public interface IDailyReadingStatisticsRepository : IRepository<DailyReadingStatistics>
    {
    }
}