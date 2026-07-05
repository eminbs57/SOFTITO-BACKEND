using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Ekipmanlar
{
    public class EditModel : PageModel
    {
        public Ekipman E { get; set; } = new Ekipman();

        public void OnGet()
        {
            string id = Request.Query["id"];
            string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Ekipmanlar WHERE ID=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            E.ID = reader["ID"].ToString();
                            E.Ad = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            E.Durum = reader.IsDBNull(2) ? "" : reader.GetString(2);

                            E.SatinAlmaTarihi = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        }
                    }
                }
            }
        }
        public void OnPost()
        {
            E.ID = Request.Form["id"];
            E.Ad = Request.Form["ad"];
            E.Durum = Request.Form["durum"];
            E.SatinAlmaTarihi = Request.Form["tarih"];

            string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Ekipmanlar SET Ad=@ad, Durum=@durum, SatinAlmaTarihi=@tarih WHERE ID=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ad", E.Ad ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@durum", E.Durum ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@tarih", string.IsNullOrEmpty(E.SatinAlmaTarihi) ? (object)DBNull.Value : E.SatinAlmaTarihi);
                    command.Parameters.AddWithValue("@id", E.ID);

                    command.ExecuteNonQuery();
                }
            }
            Response.Redirect("/Admin/Ekipmanlar/Index");
        }
    }
}