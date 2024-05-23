using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Data.Views;
using ViL.Services.Infrastructure;
using System.Linq.Expressions;
using ViL.Common.Exceptions;
using ViL.Common.Enums;

namespace ViL.Services.Services
{
    public interface IBookInfoService: IServices<BookInfo>
    {
        BookDetailsDTO GetBookDetails(string id);
        IQueryable<BookDetailsDTO> GetAllDetails();
        IQueryable<BookInfo> GetBookHasBookmarks(string userId);
        IQueryable<BookInfo> GetFollowingBook(string userId);
        void UnlockBook(string bookId);
    }
    public class BookInfoService : ServiceBase<BookInfo>, IBookInfoService
    {
        private IBookStatisticsInfoRepository _bookStatisticsInfoRepository;
        private IGenresRepository _genresRepository;
        private IUsersRepository _usersRepository;
        private IBookChaptersRepository _bookChaptersRepository;
        private IBookmarksRepository _bookmarksRepository;
        private IUserFavoriteBooksRepository _followingBooksRepository;

        public BookInfoService(IBookInfoRepository bookInfoRepository,
                               IBookStatisticsInfoRepository bookStatisticRepo,
                               IBookChaptersRepository bookChaptersRepository,
                               IGenresRepository genresRepo,
                               IUsersRepository userRepo,
                               IBookmarksRepository bookmarksRepository,
                               IUserFavoriteBooksRepository followingBooksRepository,
                               ViLDbContext dbContext) : base(bookInfoRepository, dbContext)
        {
            _bookStatisticsInfoRepository = bookStatisticRepo;
            _genresRepository = genresRepo;
            _usersRepository = userRepo;
            _bookChaptersRepository = bookChaptersRepository;
            _bookmarksRepository = bookmarksRepository;
            _followingBooksRepository = followingBooksRepository;
        }

        public override void Add(BookInfo entity)
        {
            base.Add(entity);
            _bookStatisticsInfoRepository.Add(new BookStatisticsInfo(entity.BookId));
        }

        public override void Delete(BookInfo entity)
        {
            base.Delete(entity);
            _bookStatisticsInfoRepository.Delete(stats => stats.BookId == entity.BookId);
            _bookChaptersRepository.Delete(chapter => chapter.BookId == entity.BookId);
        }

        public override void Delete(Expression<Func<BookInfo, bool>> where)
        {
            base.Delete(where);
            Expression<Func<BookStatisticsInfo, bool>> statWhere = stat => where.Compile().Invoke(new BookInfo
            {
                BookId = stat.BookId
            });
            _bookStatisticsInfoRepository.Delete(statWhere);
        }

        public IQueryable<BookDetailsDTO> GetAllDetails()
        {
            var query = from book in _repository.Table
                        join genre in _genresRepository.Table on book.GenreId equals genre.GenreId
                        join uploader in _usersRepository.Table on book.UploaderId equals uploader.UserId
                        join stats in _bookStatisticsInfoRepository.Table on book.BookId equals stats.BookId
                        where book.Status != (int)BookStatus.Locked
                        select new BookDetailsDTO(book, genre, stats, uploader);
            return query;
        }

        public BookDetailsDTO GetBookDetails(string id)
        {
            var book = _repository.GetById(id);
            var stats = _bookStatisticsInfoRepository.Get(stat => stat.BookId == id).First();
            var genre = book != null ? _genresRepository.GetById(book.GenreId) : null;
            var uploader = book != null ? _usersRepository.GetById(book.UploaderId) : null;
            if (book == null)
            {
                return new BookDetailsDTO();
            }
            var bookDetails = new BookDetailsDTO(book,genre,stats,uploader);
            return bookDetails;
        }

        public IQueryable<BookInfo> GetBookHasBookmarks(string userId)
        {
            var bookmarks = _bookmarksRepository.Get(bm => bm.UserId == userId);
            var query = from book in _repository.Table
                        join bookmark in bookmarks on book.BookId equals bookmark.BookId
                        select book;
            return query.Distinct();
        }


        public IQueryable<BookInfo> GetFollowingBook(string userId)
        {
            var query = from book in _repository.Table
                        join following in _followingBooksRepository.Table on book.BookId equals following.BookId
                        where following.UserId == userId && following.Status == 1
                        select book;
            return query;
        }

        public void UnlockBook(string bookId)
        {
            var query = _repository.GetById(bookId);
            if (query == null)
            {
                throw new VilNotFoundExceptions("Truyện không tồn tại");
            } else
            {
                query.Status = (int)BookStatus.Ongoing;
                query.LockedExpired = null;
                query.LockedReason = string.Empty;
                _repository.Update(query);
            }
        }
    }
}
