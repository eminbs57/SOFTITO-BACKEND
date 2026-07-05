using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System;

namespace HastaneRandevu.Models
{
    public class Context
    {

        public static string connectionstring = @"Server=localhost,1433;Database=HastaneDB;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=True;";

        public static void InitDatabase()
        {
            try
            {
                using (SqlConnection db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    
                    // 1. Admin Tablosu
                    db.Execute(@"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Admin' and xtype='U')
                        BEGIN
                            CREATE TABLE Admin (
                                AdminID INT IDENTITY(1,1) PRIMARY KEY,
                                Username NVARCHAR(50) NOT NULL UNIQUE,
                                PasswordHash NVARCHAR(256) NOT NULL,
                                CreatedAt DATETIME DEFAULT GETDATE()
                            )
                        END
                    ");

                    // 2. Admin Register Procedure
                    db.Execute(@"
                        CREATE OR ALTER PROCEDURE AdminRegister
                            @Username NVARCHAR(50),
                            @PasswordHash NVARCHAR(256)
                        AS
                        BEGIN
                            INSERT INTO Admin (Username, PasswordHash)
                            VALUES (@Username, @PasswordHash)
                        END
                    ");

                    // 3. Admin Login Procedure
                    db.Execute(@"
                        CREATE OR ALTER PROCEDURE AdminLogin
                            @Username NVARCHAR(50),
                            @PasswordHash NVARCHAR(256)
                        AS
                        BEGIN
                            SELECT AdminID, Username, CreatedAt 
                            FROM Admin 
                            WHERE Username = @Username AND PasswordHash = @PasswordHash
                        END
                    ");

                    // 4. Rapor Procedure
                    db.Execute(@"
                        CREATE OR ALTER PROCEDURE RandevuRapor
                        AS
                        BEGIN
                            SELECT 
                                R.RandevuNo,
                                R.RandevuTarihi,
                                R.Durum,
                                H.TCKimlikNo,
                                H.Ad + ' ' + H.Soyad AS HastaAdSoyad,
                                H.Cinsiyet,
                                H.DogumTarihi,
                                D.Ad + ' ' + D.Soyad AS DoktorAdSoyad,
                                P.PoliklinikAdi
                            FROM Randevu R
                            INNER JOIN Hasta H ON R.HastaNo = H.HastaNo
                            INNER JOIN Doktor D ON R.DoktorNo = D.DoktorNo
                            INNER JOIN Poliklinik P ON D.PoliklinikNo = P.PoliklinikNo
                            ORDER BY R.RandevuTarihi DESC
                        END
                    ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Veritabanı başlatılırken hata oluştu: " + ex.Message);
            }
        }

        public static void ExecuteReturn(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();
                db.Execute(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<T> Listeleme<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();
                return db.Query<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}