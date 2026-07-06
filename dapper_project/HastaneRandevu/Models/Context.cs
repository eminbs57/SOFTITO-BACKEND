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
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AdminRegister')
                        BEGIN
                            EXEC('CREATE PROCEDURE AdminRegister AS BEGIN SET NOCOUNT ON; END')
                        END
                    ");
                    db.Execute(@"
                        ALTER PROCEDURE AdminRegister
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
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AdminLogin')
                        BEGIN
                            EXEC('CREATE PROCEDURE AdminLogin AS BEGIN SET NOCOUNT ON; END')
                        END
                    ");
                    db.Execute(@"
                        ALTER PROCEDURE AdminLogin
                            @Username NVARCHAR(50),
                            @PasswordHash NVARCHAR(256)
                        AS
                        BEGIN
                            SELECT AdminID, Username, CreatedAt 
                            FROM Admin 
                            WHERE Username = @Username AND PasswordHash = @PasswordHash
                        END
                    ");

                    // 4. Rapor 1: Poliklinik Dağılımı
                    db.Execute(@"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_RaporPoliklinik')
                            EXEC('CREATE PROCEDURE SP_RaporPoliklinik AS BEGIN SET NOCOUNT ON; END')
                    ");
                    db.Execute(@"
                        ALTER PROCEDURE SP_RaporPoliklinik AS
                        BEGIN
                            SELECT P.PoliklinikAdi, COUNT(R.RandevuNo) AS ToplamRandevu
                            FROM Poliklinik P
                            LEFT JOIN Doktor D ON P.PoliklinikNo = D.PoliklinikNo
                            LEFT JOIN Randevu R ON D.DoktorNo = R.DoktorNo
                            GROUP BY P.PoliklinikNo, P.PoliklinikAdi
                            ORDER BY ToplamRandevu DESC
                        END
                    ");

                    // 5. Rapor 2: Doktor İstatistikleri
                    db.Execute(@"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_RaporDoktor')
                            EXEC('CREATE PROCEDURE SP_RaporDoktor AS BEGIN SET NOCOUNT ON; END')
                    ");
                    db.Execute(@"
                        ALTER PROCEDURE SP_RaporDoktor AS
                        BEGIN
                            SELECT D.AdSoyad AS DoktorAdSoyad, P.PoliklinikAdi,
                                COUNT(R.RandevuNo) AS ToplamRandevu,
                                SUM(CASE WHEN R.Durum = 'Aktif' THEN 1 ELSE 0 END) AS AktifRandevu,
                                SUM(CASE WHEN R.Durum = 'Tamamlandı' THEN 1 ELSE 0 END) AS TamamlananRandevu,
                                SUM(CASE WHEN R.Durum = 'İptal' THEN 1 ELSE 0 END) AS IptalRandevu
                            FROM Doktor D
                            INNER JOIN Poliklinik P ON D.PoliklinikNo = P.PoliklinikNo
                            LEFT JOIN Randevu R ON D.DoktorNo = R.DoktorNo
                            GROUP BY D.DoktorNo, D.AdSoyad, P.PoliklinikAdi
                            ORDER BY ToplamRandevu DESC
                        END
                    ");

                    // 6. Rapor 3: En Aktif Hastalar
                    db.Execute(@"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_RaporAktifHasta')
                            EXEC('CREATE PROCEDURE SP_RaporAktifHasta AS BEGIN SET NOCOUNT ON; END')
                    ");
                    db.Execute(@"
                        ALTER PROCEDURE SP_RaporAktifHasta AS
                        BEGIN
                            SELECT TOP 5 H.AdSoyad AS HastaAdSoyad, H.Yas, COUNT(R.RandevuNo) AS ToplamRandevu
                            FROM Hasta H
                            LEFT JOIN Randevu R ON H.HastaNo = R.HastaNo
                            GROUP BY H.HastaNo, H.AdSoyad, H.Yas
                            ORDER BY ToplamRandevu DESC
                        END
                    ");

                    // 7. Rapor 4: Randevu Durum Özeti
                    db.Execute(@"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_RaporDurum')
                            EXEC('CREATE PROCEDURE SP_RaporDurum AS BEGIN SET NOCOUNT ON; END')
                    ");
                    db.Execute(@"
                        ALTER PROCEDURE SP_RaporDurum AS
                        BEGIN
                            SELECT Durum, COUNT(RandevuNo) AS ToplamSayi
                            FROM Randevu
                            GROUP BY Durum
                            ORDER BY ToplamSayi DESC
                        END
                    ");

                    // 8. Rapor 5: En Popüler Doktorlar
                    db.Execute(@"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_RaporPopulerDoktor')
                            EXEC('CREATE PROCEDURE SP_RaporPopulerDoktor AS BEGIN SET NOCOUNT ON; END')
                    ");
                    db.Execute(@"
                        ALTER PROCEDURE SP_RaporPopulerDoktor AS
                        BEGIN
                            SELECT TOP 5 D.AdSoyad AS DoktorAdSoyad, COUNT(R.RandevuNo) AS ToplamRandevu
                            FROM Doktor D
                            LEFT JOIN Randevu R ON D.DoktorNo = R.DoktorNo
                            GROUP BY D.DoktorNo, D.AdSoyad
                            ORDER BY ToplamRandevu DESC
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