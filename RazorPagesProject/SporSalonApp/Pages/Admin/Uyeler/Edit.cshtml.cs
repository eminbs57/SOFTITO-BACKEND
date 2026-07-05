using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Uyeler
{
    public class EditModel : PageModel
    {
        public Uye UyeBilgi { get; set; } = new Uye();
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            string id = Request.Query["id"];
            string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Uyeler WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UyeBilgi.ID = reader.GetInt32(0).ToString();
                                UyeBilgi.AdSoyad = reader.GetString(1);
                                UyeBilgi.Email = reader.GetString(2);
                                UyeBilgi.Telefon = reader.GetString(3);
                                UyeBilgi.KayitTarihi = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public void OnPost()
        {
            UyeBilgi.ID = Request.Form["id"];
            UyeBilgi.AdSoyad = Request.Form["AdSoyad"];
            UyeBilgi.Email = Request.Form["email"];
            UyeBilgi.Telefon = Request.Form["telefon"];
            UyeBilgi.KayitTarihi = Request.Form["kayittarihi"];

            try
            {
                string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Uyeler SET AdSoyad=@adsoyad, Email=@email, Telefon=@telefon, KayitTarihi=@kayittarihi WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@adsoyad", UyeBilgi.AdSoyad);
                        command.Parameters.AddWithValue("@email", UyeBilgi.Email);
                        command.Parameters.AddWithValue("@telefon", UyeBilgi.Telefon);
                        command.Parameters.AddWithValue("@kayittarihi", UyeBilgi.KayitTarihi);
                        command.Parameters.AddWithValue("@id", UyeBilgi.ID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Admin/Uyeler/Index");
        }
    }
}