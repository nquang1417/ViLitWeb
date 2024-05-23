﻿using ViL.Data.Models;

namespace ViL.Api.Models
{
    public class UserInfo : EntityBase
    {
        public string UserId { get; set; }
        public string Username { get; set; }
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

        public UserInfo() : base()
        {
            UserId = string.Empty;
            Username = string.Empty;
            Role = 1;
            DisplayName = string.Empty;
            Avatar = string.Empty;
            Cover = string.Empty;
            Gender = 0;
            Email = string.Empty;
            Location = string.Empty;
            About = null;
            Coins = 0;
            Comments = 0;
            UploadItems = 0;
            FollowingItems = 0;
            HourReading = null;
            Url = null;            
            BannedExpired = null;
        }

        public UserInfo(Users user)
        {
            UserId = user.UserId;
            Username = user.Username;
            Role = user.Role;
            DisplayName = user.DisplayName;
            Avatar = user.Avatar;
            Cover = user.Cover;
            Gender = user.Gender;
            Email = user.Email;
            Location = user.Location;
            About = user.About;
            Coins = user.Coins;
            Comments = user.Comments;
            UploadItems = user.UploadItems;
            FollowingItems = user.FollowingItems;
            HourReading = user.HourReading;
            Url = user.Url;
            About = user.About;
            Coins = user.Coins;
            Comments = user.Comments;
            UploadItems = user.UploadItems;
            FollowingItems = user.FollowingItems;
            Status = user.Status;
            UpdateBy = user.UpdateBy;
            UpdateDate = user.UpdateDate;
            CreateBy = user.CreateBy;
            CreateDate = user.CreateDate;
            BannedExpired = user.BannedExpired;
        }
    }
}
