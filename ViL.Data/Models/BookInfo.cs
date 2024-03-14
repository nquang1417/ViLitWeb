using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("BookInfo")]
    public class BookInfo : EntityBase
    {
        [Key]
        public string BookId { get; set; }
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string GenreId { get; set; }
        public string? Description { get; set; }
        public string? BookCover { get; set; }
        public string? AuthorName { get; set; }
        public string UploaderId { get; set; }
        public int? Chapters { get; set; }
        public int? LanguageCode { get; set; }
        public string? LanguageName { get; set; }
        public string? Url { get; set; }

        public BookInfo() : base()
        {
            BookId = Guid.NewGuid().ToString();
            BookTitle = string.Empty;
            GenreId = string.Empty;
            UploaderId = string.Empty;
        }

        public BookInfo(string bookTitle, string genreId, string uploaderId) : base()
        {
            BookId = Guid.NewGuid().ToString();
            BookTitle = bookTitle;
            GenreId = genreId;
            UploaderId = uploaderId;
            Description = string.Empty;
            AuthorName = string.Empty;
            BookCover = string.Empty;
            Chapters = 0;
            LanguageCode = 0;
            LanguageName = string.Empty;
            Url = BookId;
        }
    }
}
