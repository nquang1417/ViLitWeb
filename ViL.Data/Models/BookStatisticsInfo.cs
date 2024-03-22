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

        public BookStatisticsInfo(string BookId) : base() 
        {
            this.BookId = BookId;
            this.Views = 0;
            this.Followers = 0;
            this.Reviews = 0;
            this.Comments = 0;
            this.AverageRating = 0;
        }
    }
}
