using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Duyurular
{
    public class IndexModel : PageModel
    {
        public List<Duyuru> Liste { get; set; } = new List<Duyuru>();
        // Aramada ne yazdığını sayfada tutmak için:
        public string SearchTerm { get; set; } = "";

        public void OnGet(string search)
        {
            SearchTerm = search;
            string connectionString = "Server=localhost,1433;Database=SporSalonuDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Arama varsa WHERE, yoksa tümünü getir
                    string sql = "SELECT * FROM Duyurular";
                    if (!string.IsNullOrEmpty(search))
                    {
                        sql += " WHERE Baslik LIKE @search OR Icerik LIKE @search";
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(search))
                        {
                            command.Parameters.AddWithValue("@search", "%" + search + "%");
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Duyuru d = new Duyuru
                                {
                                    ID = reader["ID"].ToString(),
                                    Baslik = reader["Baslik"]?.ToString() ?? "",
                                    Icerik = reader["Icerik"]?.ToString() ?? "",
                                    Tarih = reader["Tarih"]?.ToString() ?? ""
                                };
                                Liste.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Hata: " + ex.Message); }
        }
    }
}