using System.Linq.Expressions;
using ViL.Data.Models;

namespace ViL.Api.Models
{
    public class FilterBookmark
    {
        public string? UserId { get; set; }
        public string? BookId { get; set;}
    }
}
