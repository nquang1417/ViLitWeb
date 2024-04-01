using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("BookStatisticsInfo")]
    public class BookStatisticsInfo : EntityBase
    {
        [Required(ErrorMessage = "BookId is required")]
        public string BookId { get; set; }
        public int? Views { get; set; }
        public int? Followers { get; set; }
        public int? Reviews { get; set; }
        public int? Comments { get; set; }
        public double? AverageRating { get; set; }

        public BookStatisticsInfo() : base()
        {
            BookId = "";
            Views = 0;
            Followers = 0;
            Reviews = 0;
            Comments = 0;
            AverageRating = 0;
        }

        public BookStatisticsInfo(string bookId) : base() 
        {
            BookId = bookId;
            Views = 0;
            Followers = 0;
            Reviews = 0;
            Comments = 0;
            AverageRating = 0;
        }
    }
}
