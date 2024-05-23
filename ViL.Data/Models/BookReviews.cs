using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("BookReviews")]
    public class BookReviews : EntityBase
    {
        [Key]
        public string ReviewId { get; set; }
        [Required(ErrorMessage = "BookId is required")]
        public string BookId { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "ReviewContent is required")]
        public string ReviewContent { get; set; }
        [Required(ErrorMessage = "RatingScore is required")]
        public float RatingScore { get; set; }
        public int? Likes { get; set; }
        public int? Replies { get; set; }
        public string? ParentReviewId { get; set; }

        public BookReviews() : base()
        {
            ReviewId = Guid.NewGuid().ToString();
            BookId = string.Empty;
            UserId = string.Empty;
            ReviewContent = string.Empty;
            RatingScore = 0;
        }

        public BookReviews(string bookId, string userId,  string reviewContent, float score, string? parrent = null) : base()
        {
            ReviewId = Guid.NewGuid().ToString();
            BookId = bookId;
            UserId = userId;
            ReviewContent = reviewContent;
            RatingScore = score;
            Likes = 0;
            Replies = 0;
            ParentReviewId = parrent;
        }
    }
}
