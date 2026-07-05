using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SporSalonApp.Models;

namespace SporSalonApp.Pages.Admin.Ekipmanlar
{
    public class IndexModel : PageModel
    {
        public List<Ekipman> Liste { get; set; } = new List<Ekipman>();
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

                    // Arama varsa WHERE ekle, yoksa tümünü getir
                    string sql = "SELECT * FROM Ekipmanlar";
                    if (!string.IsNullOrEmpty(search))
                    {
                        sql += " WHERE Ad LIKE @search OR Durum LIKE @search";
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
                                Ekipman e = new Ekipman
                                {
                                    ID = reader["ID"].ToString(),
                                    Ad = reader["Ad"]?.ToString() ?? "",
                                    Durum = reader["Durum"]?.ToString() ?? "",
                                    SatinAlmaTarihi = reader["SatinAlmaTarihi"]?.ToString() ?? ""
                                };
                                Liste.Add(e);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Hata: " + ex.Message); }
        }
    }
}