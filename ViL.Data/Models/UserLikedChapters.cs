using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{
    [Table("UserLikedChapters")]
    public class UserLikedChapters : EntityBase
    {
        public string ChapterId { get; set; }
        public string UserId { get; set; }

        public UserLikedChapters() : base()
        {
            ChapterId = "";
            UserId = "";
        }

        public UserLikedChapters(string chapterId = "", string userId = "") : base()
        {
            ChapterId = chapterId;
            UserId = userId;
        }
    }
}
