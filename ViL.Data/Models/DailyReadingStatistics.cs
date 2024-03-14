using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("DailyReadingStatistics")]
    public class DailyReadingStatistics : EntityBase
    {
        [Key]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? TotalReadingTime { get; set; }

        public DailyReadingStatistics(string userId) : base()
        {
            UserId = userId;
            Date = DateTime.Now;
            TotalReadingTime = TimeSpan.Zero;
        }
    }
}
