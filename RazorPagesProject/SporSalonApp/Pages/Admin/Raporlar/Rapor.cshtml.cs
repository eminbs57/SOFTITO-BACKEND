using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SporSalonApp.Pages.Admin.Raporlar
{
    public class RaporModel : PageModel
    {
        public int UyeSayisi { get; set; }
        public int DuyuruSayisi { get; set; }
        public int EkipmanSayisi { get; set; }

        public void OnGet()
        {
            string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                UyeSayisi = GetCount("SELECT COUNT(*) FROM Uyeler", connection);
                DuyuruSayisi = GetCount("SELECT COUNT(*) FROM Duyurular", connection);
                EkipmanSayisi = GetCount("SELECT COUNT(*) FROM Ekipmanlar", connection);
            }
        }

        private int GetCount(string sql, SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}