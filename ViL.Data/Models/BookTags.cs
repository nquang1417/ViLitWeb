
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("BookTags")]
    public class BookTags : EntityBase
    {
        public string BookId { get; set; }
        public string TagId { get; set; }

        public BookTags(string bookId, string tagId) : base() 
        {
            BookId = bookId;
            TagId = tagId;
        }
    }
}
