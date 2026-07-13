using System.Collections.Generic;

namespace ObiletApp.Core.Interfaces
{
    public interface IReportService
    {
        byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1");
        byte[] GeneratePdf(string htmlContent);
    }
}
