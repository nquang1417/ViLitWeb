using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViL.Common.Commons;

namespace ViL.Data.Models
{
    [Table("UserFavoriteBooks")]
    [HasNoKey]
    public class UserFavoriteBooks : EntityBase
    {
        [VilUnchanged]
        [AsKey]
        public string BookId { get; set; }
        [VilUnchanged]
        [AsKey]
        public string UserId { get; set; }

        public UserFavoriteBooks() : base()
        {
            BookId = "";
            UserId = "";
        }

        public UserFavoriteBooks(string bookId = "", string userId = "") : base(status: 1, creaeteBy: userId, updateBy: userId)
        {
            BookId = bookId;
            UserId = userId;
        }
        
    }
}
