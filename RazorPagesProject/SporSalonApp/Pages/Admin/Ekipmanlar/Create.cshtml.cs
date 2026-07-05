using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Ekipmanlar
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Ekipman E { get; set; } = new Ekipman();
        public string ErrorMessage { get; set; } = "";

        public void OnPost()
        {
            E.Ad = Request.Form["ad"];
            E.Durum = Request.Form["durum"];
            E.SatinAlmaTarihi = Request.Form["tarih"];

            try
            {
                string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Ekipmanlar (Ad, Durum, SatinAlmaTarihi) VALUES (@ad, @durum, @tarih)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ad", E.Ad);
                        command.Parameters.AddWithValue("@durum", E.Durum);
                        command.Parameters.AddWithValue("@tarih", E.SatinAlmaTarihi);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { ErrorMessage = ex.Message; return; }
            Response.Redirect("/Admin/Ekipmanlar/Index");
        }
    }
}