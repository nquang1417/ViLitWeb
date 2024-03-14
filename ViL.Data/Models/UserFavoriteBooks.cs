using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("UserFavoriteBooks")]
    public class UserFavoriteBooks : EntityBase
    {
        public string BookId { get; set; }
        public string UserId { get; set; }

        public UserFavoriteBooks() : base()
        {
            BookId = "";
            UserId = "";
        }

        public UserFavoriteBooks(string bookId = "", string userId = "") : base()
        {
            BookId = bookId;
            UserId = userId;
        }
        
    }
}
