using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookStatisticsInfoService : IServices<BookStatisticsInfo>
    {
    }

    public class BookStatisticsInfoService : ServiceBase<BookStatisticsInfo>, IBookStatisticsInfoService
    {
        public BookStatisticsInfoService(IBookStatisticsInfoRepository repository, ViLDbContext dbContext) : base(repository, dbContext) { }

        protected override bool validate(BookStatisticsInfo entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);

            }
            var isValid = true;
            var query = _repository.Get(bs => bs.BookId == entity.BookId).First();
            if (query == null)
            {
                isValid = false;
                throw new VilNotFoundExceptions("Thông tin không tồn tại");
            }
            return isValid;
        }

    }
}
