
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViL.Common.Commons;

namespace ViL.Data.Models
{
    [Table("BookTags")]
    [HasNoKey]
    public class BookTags : EntityBase
    {
        [VilUnchanged]
        [AsKey]
        public string BookId { get; set; }
        [VilUnchanged]
        [AsKey]
        public string TagId { get; set; }

        public BookTags(string bookId, string tagId) : base() 
        {
            BookId = bookId;
            TagId = tagId;
        }
    }
}
