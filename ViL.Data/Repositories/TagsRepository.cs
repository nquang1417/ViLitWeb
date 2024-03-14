using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class TagsRepository : RepositoryBase<Tags>, ITagsRepository
    {
       public TagsRepository(ViLDbContext context) : base(context) { }
    }

    public interface ITagsRepository : IRepository<Tags>
    {
    }
}