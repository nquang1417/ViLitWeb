using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{

    [Table("Reports")]
    public class Reports : EntityBase
    {
        [Key]
        public string ReportsId { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public int ReportedType { get; set; }
        [Required]
        public string ReportedEntityId { get; set; }
        public DateTime ReportedDate { get; set; }
        public string? ReportContent { get; set; }
        public string? Response { get; set; }

        public Reports(string senderId, int reportedType, string reportedEntityId)
        {
            ReportsId = Guid.NewGuid().ToString();
            SenderId = senderId;
            ReportedType = reportedType;
            ReportedEntityId = reportedEntityId;
            ReportedDate = DateTime.Now;
            ReportContent = "";
            Response = "";
        }
    }
}
