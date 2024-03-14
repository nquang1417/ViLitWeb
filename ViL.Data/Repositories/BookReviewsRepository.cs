using ViL.Data.Infrastructure;
using ViL.Data.Models;

namespace ViL.Data.Repositories
{
    public class BookReviewsRepository : RepositoryBase<BookReviews>, IBookReviewsRepository
    {
       public BookReviewsRepository(ViLDbContext context) : base(context) { }
    }

    public interface IBookReviewsRepository : IRepository<BookReviews>
    {
    }
}