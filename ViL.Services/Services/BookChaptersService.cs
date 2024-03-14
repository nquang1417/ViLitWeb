using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;

namespace ViL.Services.Services
{
    public interface IBookChaptersService
    {
        public List<BookChapters> GetBookChapters(string bookId);
        public BookChapters GetById(string id);
        public void AddChapter(BookChapters bookChapters);
        public void UpdateChapter(BookChapters bookChapters);
        public void DeleteChapter(BookChapters bookChapters);
        public void Delete(Expression<Func<Users, bool>> where);
        public void ChangeStatus(string id,  string status);
    }

    public class BookChaptersService : IBookChaptersService
    {
        private IBookChaptersRepository _chapterRepository;
        private ViLDbContext _dbContext;

        public BookChaptersService(IBookChaptersRepository chapterRepository, ViLDbContext dbContext)
        {
            _chapterRepository = chapterRepository;
            _dbContext = dbContext;
        }

        public void AddChapter(BookChapters bookChapters)
        {
            throw new NotImplementedException();
        }

        public void ChangeStatus(string id, string status)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Users, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void DeleteChapter(BookChapters bookChapters)
        {
            throw new NotImplementedException();
        }

        public List<BookChapters> GetBookChapters(string bookId)
        {
            throw new NotImplementedException();
        }

        public BookChapters GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateChapter(BookChapters bookChapters)
        {
            throw new NotImplementedException();
        }

        public bool validate(BookChapters chapter, bool isUpdate = false)
        {
            if (chapter == null)
            {
                return false;
            }
            var isValid = true;
            if (chapter.BookId.IsNullOrEmpty())
            {
                isValid = false;
            }
            
            return isValid;
        }
    }
}
