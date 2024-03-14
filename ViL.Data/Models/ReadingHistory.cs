using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("ReadingHistory")]
    public class ReadingHistory : EntityBase
    {
        [Key]
        public string ReadingId { get; set; }
        public string UserId { get; set; }        
        public string LastChapterId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public ReadingHistory() : base()
        {
            ReadingId = Guid.NewGuid().ToString();
            UserId = "";
            LastChapterId = "";
        }

        public ReadingHistory(string userId, string lastChapterId)
        {
            ReadingId = Guid.NewGuid().ToString();
            UserId = userId;
            LastChapterId = lastChapterId;
        }
    }
}
