using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class BookStatisticsInfoRepository : RepositoryBase<BookStatisticsInfo>, IBookStatisticsInfoRepository
    {
        public BookStatisticsInfoRepository(ViLDbContext context) : base(context) { }
    }

    public interface IBookStatisticsInfoRepository : IRepository<BookStatisticsInfo>
    {
    }
}