using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViL.Common.Commons;

namespace ViL.Data.Models
{
    [Table("UserLikedComments")]
    [HasNoKey]
    public class UserLikedComments : EntityBase
    {
        [VilUnchanged]
        [AsKey]
        public string CommentId { get; set; }
        [VilUnchanged]
        [AsKey]
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
