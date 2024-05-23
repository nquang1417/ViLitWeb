using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViL.Data.Models
{

    [Table("Users")]
    public class Users : EntityBase
    {
        [Key]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public short Role { get; set; }
        public string? DisplayName { get; set; }
        public string? Avatar { get; set; }
        public string? Cover { get; set; }
        public short? Gender { get; set; }
        public string? Email { get; set; }
        public string? Location { get; set; }
        public string? About { get; set; }
        public int? Coins { get; set; }
        public int? Comments { get; set; }
        public int? UploadItems { get; set; }
        public int? FollowingItems { get; set; }
        public TimeSpan? HourReading { get; set; }
        public string? Url { get; set; }
        public DateTime? BannedExpired { get; set; }
        
        public Users() : base()
        {
            UserId = Guid.NewGuid().ToString();
            Username = string.Empty;
            Password = string.Empty;
            Role = 1;
        }

        public Users(string username, string password, short role = 1) : base()
        {
            UserId = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            Role = role;
            DisplayName = username;
            Avatar = string.Empty;
            Cover = string.Empty;
            Gender = -1;
            Email = string.Empty;
            Location = string.Empty;
            About = string.Empty;
            Coins = 0;
            Comments = 0;
            UploadItems = 0;
            FollowingItems = 0;
            HourReading = TimeSpan.Zero;
            Url = UserId + "_" + username;
            BannedExpired = null;
        }
    }
}
