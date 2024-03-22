using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class NotificationsRepository : RepositoryBase<Notifications>, INotificationsRepository
    {
       public NotificationsRepository(ViLDbContext context) : base(context) { }
    }

    public interface INotificationsRepository : IRepository<Notifications>
    {
    }
}