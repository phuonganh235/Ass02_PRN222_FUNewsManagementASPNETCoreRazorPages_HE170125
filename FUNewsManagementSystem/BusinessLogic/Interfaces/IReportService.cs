using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public class ReportRow
    {
        public string GroupName { get; set; } = "";
        public int TotalArticles { get; set; }
        public int ActiveArticles { get; set; }
        public int InactiveArticles { get; set; }
        public DateTime LastCreatedDate { get; set; }
    }

    public interface IReportService
    {
        Task<List<ReportRow>> GetReportAsync(DateTime? start, DateTime? end, string groupBy);
    }
}
