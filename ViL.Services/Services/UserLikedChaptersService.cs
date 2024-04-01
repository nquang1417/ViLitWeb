using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IUserLikedChaptersService : IServices<UserLikedChapters>
    {
    }

    public class UserLikedChaptersService : ServiceBase<UserLikedChapters>, IUserLikedChaptersService
    {
        public UserLikedChaptersService(IUserLikedChaptersRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(UserLikedChapters entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.Get(obj => obj.UserId == entity.UserId && obj.ChapterId == entity.ChapterId).First();
            if (query == null)
            {
                isValid = false;
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
