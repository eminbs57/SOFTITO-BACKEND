using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Duyurular
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Duyuru DuyuruBilgi { get; set; } = new Duyuru();
        public string ErrorMessage { get; set; } = "";

        public void OnGet() { }

        public void OnPost()
        {
            DuyuruBilgi.Baslik = Request.Form["baslik"];
            DuyuruBilgi.Icerik = Request.Form["icerik"];
            DuyuruBilgi.Tarih = Request.Form["tarih"];

            if (string.IsNullOrEmpty(DuyuruBilgi.Baslik) || string.IsNullOrEmpty(DuyuruBilgi.Icerik))
            {
                ErrorMessage = "Başlık ve İçerik alanları zorunludur!";
                return;
            }

            try
            {
                string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Duyurular (Baslik, Icerik, Tarih) VALUES (@baslik, @icerik, @tarih)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@baslik", DuyuruBilgi.Baslik);
                        command.Parameters.AddWithValue("@icerik", DuyuruBilgi.Icerik);
                        command.Parameters.AddWithValue("@tarih", DuyuruBilgi.Tarih);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Admin/Duyurular/Index");
        }
    }
}