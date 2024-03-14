using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("Bookmark")]
    public class Bookmarks
    {
        [Key]
        public string BookmarkId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ChapterId { get; set; }
        public int BookmarkLine { get; set; }
        public string? Notes { get; set; }

        public Bookmarks() : base()
        {
            BookmarkId = Guid.NewGuid().ToString();
            UserId = string.Empty;
            ChapterId = string.Empty;
        }

        public Bookmarks(string userID, string chapterId) : base()
        {
            BookmarkId = Guid.NewGuid().ToString();
            UserId = userID;
            ChapterId = chapterId;
            BookmarkLine = 0;
            Notes = string.Empty;
        }
    }
}
