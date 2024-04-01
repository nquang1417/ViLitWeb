using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViL.Data.Models;

namespace ViL.Data.Views
{
    public class BookDetailsDTO : EntityBase
    {
        public string? BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? GenreId { get; set; }
        public string? GenreName { get; set; }
        public string? Description { get; set; }
        public string? BookCover { get; set; }
        public string? AuthorName { get; set; }
        public string? UploaderId { get; set; }
        public string? Username { get; set; }
        public string? Avatar { get; set; }
        public int? Chapters { get; set; }
        public int? LanguageCode { get; set; }
        public string? LanguageName { get; set; }
        public string? Url { get; set; }
        public int? Views { get; set; }
        public int? Followers { get; set; }
        public int? Reviews { get; set; }
        public int? Comments { get; set; }
        public double? AverageRating { get; set; }

        public BookDetailsDTO()
        {

        }

        public BookDetailsDTO(BookInfo book, Genres? genre, BookStatisticsInfo? stats, Users? user)
        {
            BookId = book.BookId;
            BookTitle = book.BookTitle;
            GenreId = book.GenreId;
            GenreName = genre?.GenreName;
            Description = book.Description;
            BookCover = book.BookCover; 
            AuthorName = book.AuthorName;
            UploaderId = book.UploaderId;
            Username = user?.Username;
            Avatar = user?.Avatar;
            Chapters = book.Chapters;
            LanguageCode = book.LanguageCode;
            LanguageName = book.LanguageName;
            Url = book.Url;
            Views = stats?.Views;
            Followers = stats?.Followers;
            Reviews = stats?.Reviews;
            Comments = stats?.Comments;
            AverageRating = stats?.AverageRating;
            Status = book.Status;
            UpdateBy = book.UpdateBy;
            UpdateDate = book.UpdateDate;
            CreateBy = book.CreateBy;
            CreateDate = book.CreateDate;
        }
    }
}
