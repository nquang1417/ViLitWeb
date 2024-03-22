using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("Tags")]
    public class Tags : EntityBase
    {
        [Key]
        public string TagId { get; set; }
        [Required(ErrorMessage = "TagName is required")]
        public string TagName { get; set; }
        public string? Description { get; set; }

        public Tags() : base()
        {
            TagId = Guid.NewGuid().ToString();
            TagName = "";
            Description = "";
        }

        public Tags(string tagName, string? description = "")
        {
            TagId = Guid.NewGuid().ToString();
            TagName = tagName;
            Description = description;
        }
    }
}
