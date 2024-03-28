using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface INotificationsService : IServices<Notifications>
    {
    }

    public class NotificationsService : ServiceBase<Notifications>, INotificationsService
    {
        public NotificationsService(INotificationsRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(Notifications entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.GetById(entity.NotiId);
            if (query != null)
            {
                if (query.Type != entity.Type)
                {
                    isValid = false;
                    listErrorMsgs.Add("Type không được phép thay đổi");
                }
                if (query.Recipient != entity.Recipient)
                {
                    isValid = false;
                    listErrorMsgs.Add("Recipient không được phép thay đổi");
                }
            } else
            {
                isValid = false;
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
