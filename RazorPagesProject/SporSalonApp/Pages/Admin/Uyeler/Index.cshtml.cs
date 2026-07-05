using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Uyeler
{
    public class IndexModel : PageModel
    {
        public List<Uye> Liste { get; set; } = new List<Uye>();
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

                    string sql = "SELECT * FROM Uyeler";

                    if (!string.IsNullOrEmpty(search))
                    {
                        sql += " WHERE AdSoyad LIKE @search";
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
                                Uye uye = new Uye
                                {
                                    ID = reader.GetInt32(0).ToString(),
                                    AdSoyad = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Telefon = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    KayitTarihi = reader.IsDBNull(4) ? "" : reader.GetString(4)
                                };
                                Liste.Add(uye);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
    }
}