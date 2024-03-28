using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookInfoService: IServices<BookInfo>
    {
    }
    public class BookInfoService : ServiceBase<BookInfo>, IBookInfoService
    {
        public BookInfoService(IBookInfoRepository bookInfoRepository, ViLDbContext dbContext) : base(bookInfoRepository, dbContext)
        {
        }   
    }
}
