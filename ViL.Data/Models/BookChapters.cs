using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("BookChapters")]
    public class BookChapters : EntityBase
    {
        [Key]        
        public string ChapterId { get; set; }
        [Required]
        public string BookId { get; set; }
        public string? ChapterTitle { get; set; }
        public int? ChapterNum { get; set; }
        public string? FileName { get; set; }
        public string? Images { get; set; }
        public string? Url { get; set; }
        public int? Views { get; set; }
        public int? Comments { get; set; }
        public int? Likes { get; set; }

        public BookChapters() : base()
        {
            ChapterId = Guid.NewGuid().ToString();
            BookId = string.Empty;
        }

        public BookChapters(string bookId) : base()
        {
            ChapterId = Guid.NewGuid().ToString();
            BookId = bookId;
            ChapterTitle = string.Empty;
            ChapterNum = 0;
            Images = string.Empty;
            FileName = string.Empty;
            Url = BookId;
            Views = 0;
            Comments = 0;
            Likes = 0;
        }
    }
}
