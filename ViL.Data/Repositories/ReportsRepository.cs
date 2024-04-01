using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class ReportsRepository : RepositoryBase<Reports>, IReportsRepository
    {
       public ReportsRepository(ViLDbContext context) : base(context) { }
    }

    public interface IReportsRepository : IRepository<Reports>
    {
    }
}