using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class ChapterCommentsRepository : RepositoryBase<ChapterComments>, IChapterCommentsRepository
    {
       public ChapterCommentsRepository(ViLDbContext context) : base(context) { }
    }

    public interface IChapterCommentsRepository : IRepository<ChapterComments>
    {
    }
}