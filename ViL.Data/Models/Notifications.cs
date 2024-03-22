using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViL.Common.Enums;

namespace ViL.Data.Models
{
    [Table("Notifications")]
    public class Notifications : EntityBase
    {
        [Key]
        public string NotiId { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Recipient { get; set; }

        public Notifications(int type, string recipient, string message = "") : base()
        {
            NotiId = Guid.NewGuid().ToString();
            Type = type;
            Message = message;
            Recipient = recipient;
            Status = 0;
            CreateDate = DateTime.Now;
            CreateBy = "System";
            UpdateDate = DateTime.Now;
            UpdateBy = "System";
        }
    }
}
