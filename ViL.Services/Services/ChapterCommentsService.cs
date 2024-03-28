using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IChapterCommentsService : IServices<ChapterComments>
    {
    }

    public class ChapterCommentsService : ServiceBase<ChapterComments>, IChapterCommentsService
    {
        public ChapterCommentsService(IChapterCommentsRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(ChapterComments entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.GetById(entity.CommentId);
            if (query != null)
            {
                if (query.ChapterId != entity.ChapterId)
                {
                    isValid = false;
                    listErrorMsgs.Add("ChapterId không được phép thay đổi");
                }
                if (query.UserId != entity.UserId)
                {
                    isValid = false;
                    listErrorMsgs.Add("UserId không được phép thay đổi");
                }
            } else
            {
                throw new VilNotFoundExceptions("Bình luận này không tồn tại");
            }
            return isValid;
        }
    }
}
