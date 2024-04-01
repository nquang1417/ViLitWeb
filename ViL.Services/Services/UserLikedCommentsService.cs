using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IUserLikedCommentsService : IServices<UserLikedComments>
    {
    }

    public class UserLikedCommentsService : ServiceBase<UserLikedComments>, IUserLikedCommentsService
    {
        public UserLikedCommentsService(IUserLikedCommentsRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(UserLikedComments entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.Get(obj => obj.UserId == entity.UserId && obj.CommentId == entity.CommentId).First();
            if (query == null)
            {
                isValid = false;
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
