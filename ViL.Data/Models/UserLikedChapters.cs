using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViL.Common.Commons;

namespace ViL.Data.Models
{
    [Table("UserLikedChapters")]
    [HasNoKey]
    public class UserLikedChapters : EntityBase
    {
        [VilUnchanged]
        [AsKey]
        public string ChapterId { get; set; }
        [VilUnchanged]
        [AsKey]
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
