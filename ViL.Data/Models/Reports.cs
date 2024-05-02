using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

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
        public int TargetType { get; set; }
        [Required]
        public string ReportedEntityId { get; set; }
        public DateTime ReportedDate { get; set; }
        public string? ReportContent { get; set; }
        public string? Response { get; set; }

        public Reports()
        {
            ReportsId = Guid.NewGuid().ToString();
            SenderId = string.Empty;
            ReportedType = 0;
            ReportedEntityId = string.Empty;
            TargetType = 0;
            ReportedDate = DateTime.Now;
            ReportContent = string.Empty;
            Response = string.Empty;
        }

        public Reports(string senderId, int reportedType, string reportedEntityId, int targetType)
        {
            ReportsId = Guid.NewGuid().ToString();
            SenderId = senderId;
            ReportedType = reportedType;
            ReportedEntityId = reportedEntityId;
            TargetType = targetType;
            ReportedDate = DateTime.Now;
            ReportContent = "";
            Response = "";
        }
    }
}
