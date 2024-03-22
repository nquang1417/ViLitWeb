using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("ChapterComments")]
    public class ChapterComments : EntityBase
    {
        [Key]
        public string CommentId { get; set; }
        [Required(ErrorMessage = "ChapterId is required")]
        public string ChapterId { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        
        public string CommentContent { get; set; }
        public int? Likes { get; set; }
        public int? Replies { get; set; }
        public string? ParentCommentId { get; set; }

        public ChapterComments()
        {
            CommentId = Guid.NewGuid().ToString();
            ChapterId = "";
            UserId = "";
            CommentContent = "";
            Likes = 0;
            Replies = 0;
            ParentCommentId = null;
        }

        public ChapterComments(string chapterId, string userId, string commentContent = "", string? parrent = null) : base()
        {
            CommentId = Guid.NewGuid().ToString();
            ChapterId = chapterId;
            UserId = userId;
            CommentContent = commentContent;
            Likes = 0;
            Replies = 0;
            ParentCommentId = parrent;
        }
    }
}
