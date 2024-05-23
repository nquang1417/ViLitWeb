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
        public string ReportName { get; set; }
        [Required]
        public int ReportedType { get; set; }
        [Required]
        public int TargetType { get; set; } // 0: User, 1: Novel, 2: Chapter
        [Required]
        public string ReportedEntityId { get; set; }
        public DateTime ReportedDate { get; set; }
        public string? Message { get; set; }
        public string? Response { get; set; }

        public Reports()
        {

            ReportsId = Guid.NewGuid().ToString();
            SenderId = string.Empty;
            ReportName = string.Empty;
            ReportedType = 0;
            ReportedEntityId = string.Empty;
            TargetType = 0;
            ReportedDate = DateTime.Now;
            Message = string.Empty;
            Response = string.Empty;
            Status = 0;
        }

        public Reports(string senderId, string name, int reportedType, string reportedEntityId, int targetType, string message = "")
        {
            ReportsId = Guid.NewGuid().ToString();
            SenderId = senderId;
            ReportName = name;
            ReportedType = reportedType;
            ReportedEntityId = reportedEntityId;
            TargetType = targetType;
            ReportedDate = DateTime.Now;
            Message = message;
            Response = "";
            Status = 0;
        }
    }
}
