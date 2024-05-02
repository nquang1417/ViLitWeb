using ViL.Data.Models;

namespace ViL.Api.Models
{
    public class FilterReports
    {
        public int? ReportedType { get; set; }
        public int? ReportEntityType { get; set; }
        public int? Status { get; set; }

        public FilterReports()
        {
        }

    }
    
}
