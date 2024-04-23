using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Net;
using System.Xml.Linq;
using System;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Data.Views;
using ViL.Services.Infrastructure;
using System.Linq.Expressions;

namespace ViL.Services.Services
{
    public interface IBookInfoService: IServices<BookInfo>
    {
        BookDetailsDTO GetBookDetails(string id);
        IQueryable<BookDetailsDTO> GetAllDetails();
    }
    public class BookInfoService : ServiceBase<BookInfo>, IBookInfoService
    {
        private IBookStatisticsInfoRepository _bookStatisticsInfoRepository;
        private IGenresRepository _genresRepository;
        private IUsersRepository _usersRepository;
        private IBookChaptersRepository _bookChaptersRepository;

        public BookInfoService(IBookInfoRepository bookInfoRepository,
                               IBookStatisticsInfoRepository bookStatisticRepo,
                               IBookChaptersRepository bookChaptersRepository,
                               IGenresRepository genresRepo,
                               IUsersRepository userRepo,
                               ViLDbContext dbContext) : base(bookInfoRepository, dbContext)
        {
            _bookStatisticsInfoRepository = bookStatisticRepo;
            _genresRepository = genresRepo;
            _usersRepository = userRepo;
            _bookChaptersRepository = bookChaptersRepository;
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
    }
}
