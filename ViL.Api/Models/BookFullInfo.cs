using Microsoft.AspNetCore.Mvc;
using ViL.Data.Models;

namespace ViL.Api.Models
{
    public class BookFullInfo : EntityBase
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
        public FileContentResult? Cover { get; set; }

        //[StringLength(50)]
        public string[]? Tags { get; set; }
        public double? AverageRating { get; set; }

        public BookFullInfo(BookInfo book, BookStatisticsInfo bookStats, Genres genres)
        {
            foreach (var prop in bookStats.GetProperties())
            {
                var value = prop.GetValue(bookStats);
                this.GetType().GetProperty(prop.Name)?.SetValue(this, value);
            }
            foreach (var prop in genres.GetProperties())
            {
                var value = prop.GetValue(genres);
                this.GetType().GetProperty(prop.Name)?.SetValue(this, value);
            }
            foreach (var prop in book.GetProperties())
            {
                var value = prop.GetValue(book);
                this.GetType().GetProperty(prop.Name)?.SetValue(this, value);
            }
        }

        public BookInfo GetBookInfo()
        {
            BookInfo book = new BookInfo();
            foreach (var prop in book.GetProperties())
            {
                var value = this.GetType().GetProperty(prop.Name)?.GetValue(this);
                prop.SetValue(book, value);
            }
            return book;
        }

        public BookStatisticsInfo GetBookStats()
        {
            BookStatisticsInfo bookStats = new BookStatisticsInfo();
            foreach (var prop in bookStats.GetProperties())
            {
                var value = this.GetType().GetProperty(prop.Name)?.GetValue(this);
                prop.SetValue(bookStats, value);
            }
            return bookStats;
        }

        public Genres GetGenres()
        {
            Genres genres = new Genres();
            foreach (var prop in genres.GetProperties())
            {
                var value = this.GetType().GetProperty(prop.Name)?.GetValue(this);
                prop.SetValue(genres, value);
            }
            return genres;
        }
    }
}
