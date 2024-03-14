using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("UserLikedReviews")]
    public class UserLikedReviews : EntityBase
    {
        public string ReviewId { get; set; }
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
