using System.Linq.Expressions;
using ViL.Data.Models;

namespace ViL.Api.Models
{
    public class FilterBook 
    {
        public string? Title { get; set; }
        public string? GenreId { get; set; }
        public string? AuthorName { get; set; }
        public int? Status { get; set; }
        public string? UploaderId { get; set; }

        public FilterBook(string? title = null, string? genreId = null, string? authorName = null, int? status = null, string? uploaderId = null)
        {
            Title = title;
            GenreId = genreId;
            AuthorName = authorName;
            Status = status;
            UploaderId = uploaderId;
        }

    }
}
