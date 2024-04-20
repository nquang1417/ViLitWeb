using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViL.Common.Commons;

namespace ViL.Data.Models
{
    [Table("UserLikedReviews")]
    [HasNoKey]
    public class UserLikedReviews : EntityBase
    {
        [VilUnchanged]
        [AsKey]
        public string ReviewId { get; set; }
        [VilUnchanged]
        [AsKey]
        public string UserId { get; set; }

        public UserLikedReviews() : base()
        {
            ReviewId = "";
            UserId = "";
        }

        public UserLikedReviews(string reviewId = "", string userId = "") : base()
        {
            ReviewId = reviewId;
            UserId = userId;
        }
    }
}
