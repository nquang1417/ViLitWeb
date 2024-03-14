using Microsoft.EntityFrameworkCore;
using ViL.Data.Models;

namespace ViL.Data
{
    public class ViLDbContext : DbContext
    {
        public ViLDbContext(DbContextOptions<ViLDbContext> options) : base(options) { }


        public DbSet<BookChapters> BookChapters { get; set; }
        public DbSet<BookInfo> BookInfo { get; set; }
        public DbSet<Bookmarks> Bookmarks { get; set; }
        public DbSet<BookReviews> BookReviews { get; set; }
        public DbSet<BookStatisticsInfo> BookStatisticsInfo { get; set; }
        public DbSet<BookTags> BookTags { get; set; }
        public DbSet<ChapterComments> ChapterComments { get; set; }
        public DbSet<DailyReadingStatistics> DailyReadingStatistics { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<ReadingHistory> ReadingHistory { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<UserFavoriteBooks> UserFavoriteBooks { get; set; }
        public DbSet<UserLikedChapters> UserLikedChapters { get; set; }
        public DbSet<UserLikedComments> UserLikedComments { get; set; }
        public DbSet<UserLikedReviews> UserLikedReviews { get; set; }
        public DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookTags>().HasNoKey();
            modelBuilder.Entity<UserFavoriteBooks>().HasNoKey();
            modelBuilder.Entity<UserLikedChapters>().HasNoKey();
            modelBuilder.Entity<UserLikedComments>().HasNoKey();
            modelBuilder.Entity<UserLikedReviews>().HasNoKey();
        }
    }
}
