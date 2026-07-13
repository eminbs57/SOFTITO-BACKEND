using ClosedXML.Excel;
using iText.Html2pdf;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace ObiletApp.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        public byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            worksheet.Cell(1, 1).InsertTable(data);

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            using var memoryStream = new MemoryStream();

            HtmlConverter.ConvertToPdf(htmlContent, memoryStream);

            return memoryStream.ToArray();
        }
    }
}
