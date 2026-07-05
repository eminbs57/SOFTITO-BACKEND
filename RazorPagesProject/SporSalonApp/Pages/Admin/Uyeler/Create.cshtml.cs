using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Uyeler
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Uye UyeBilgi { get; set; } = new Uye();
        public string ErrorMessage { get; set; } = "";

        public void OnGet() { }

        public void OnPost()
        {

            UyeBilgi.AdSoyad = Request.Form["adsoyad"];
            UyeBilgi.Email = Request.Form["email"];
            UyeBilgi.Telefon = Request.Form["telefon"];
            UyeBilgi.KayitTarihi = Request.Form["kayittarihi"];

            if (string.IsNullOrEmpty(UyeBilgi.AdSoyad) || string.IsNullOrEmpty(UyeBilgi.Email))
            {
                ErrorMessage = "Ad Soyad ve Email alanları zorunludur!";
                return;
            }

            try
            {
                string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Uyeler (AdSoyad, Email, Telefon, KayitTarihi) VALUES (@adsoyad, @email, @telefon, @kayittarihi)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@adsoyad", UyeBilgi.AdSoyad);
                        command.Parameters.AddWithValue("@email", UyeBilgi.Email);
                        command.Parameters.AddWithValue("@telefon", UyeBilgi.Telefon);
                        command.Parameters.AddWithValue("@kayittarihi", UyeBilgi.KayitTarihi);
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