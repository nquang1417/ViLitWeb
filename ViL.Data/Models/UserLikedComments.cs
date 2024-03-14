using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("UserLikedComments")]
    public class UserLikedComments : EntityBase
    {
        public string CommentId { get; set; }
        public string UserId { get; set; }

        public UserLikedComments() : base()
        {
            CommentId = "";
            UserId = "";
        }

        public UserLikedComments(string commentId = "", string userId = "") : base()
        {
            CommentId = commentId;
            UserId = userId;
        }
    }
}
