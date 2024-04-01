using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IReadingHistoryService : IServices<ReadingHistory>
    {
    }

    public class ReadingHistoryService : ServiceBase<ReadingHistory>, IReadingHistoryService
    {
        public ReadingHistoryService(IReadingHistoryRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(ReadingHistory entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.GetById(entity.ReadingId);
            if (query == null)
            {
                isValid = false;
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
