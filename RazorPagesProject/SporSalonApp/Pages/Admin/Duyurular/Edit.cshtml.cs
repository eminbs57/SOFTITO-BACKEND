using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Duyurular
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Duyuru DuyuruBilgi { get; set; } = new Duyuru();
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
                    string sql = "SELECT * FROM Duyurular WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DuyuruBilgi.ID = reader.GetInt32(0).ToString();
                                DuyuruBilgi.Baslik = reader.GetString(1);
                                DuyuruBilgi.Icerik = reader.GetString(2);
                                DuyuruBilgi.Tarih = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public void OnPost()
        {
            DuyuruBilgi.ID = Request.Form["id"];
            DuyuruBilgi.Baslik = Request.Form["baslik"];
            DuyuruBilgi.Icerik = Request.Form["icerik"];
            DuyuruBilgi.Tarih = Request.Form["tarih"];

            try
            {
                string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Duyurular SET Baslik=@baslik, Icerik=@icerik, Tarih=@tarih WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@baslik", DuyuruBilgi.Baslik);
                        command.Parameters.AddWithValue("@icerik", DuyuruBilgi.Icerik);
                        command.Parameters.AddWithValue("@tarih", DuyuruBilgi.Tarih);
                        command.Parameters.AddWithValue("@id", DuyuruBilgi.ID);
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