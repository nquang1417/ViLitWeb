using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("Genres")]
    public class Genres : EntityBase
    {
        [Key]
        public string GenreId { get; set; }
        [Required]
        public string GenreName { get; set; }
        public string? Description { get; set; }

        public Genres(string genreName) : base()
        {
            GenreId = Guid.NewGuid().ToString();
            GenreName = genreName;
            Description = string.Empty;
        }
    }
}
