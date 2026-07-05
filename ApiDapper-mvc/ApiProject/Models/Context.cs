using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace ApiProject.Models
{
    public class Context
    {
        public static string connectionstring = @"Server=localhost,1433;Database=OtelApiDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=True;";

        public static void ExecuteReturn(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();
                db.Execute(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static void InitializeDatabase()
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();
                
                string createTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
                BEGIN
                    CREATE TABLE Users (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        FullName NVARCHAR(100),
                        Email NVARCHAR(100) UNIQUE,
                        PasswordHash NVARCHAR(MAX)
                    );
                END";
                db.Execute(createTable);

                string createRegisterProc = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID('dbo.UserRegister'))
                BEGIN
                    EXEC('CREATE PROCEDURE dbo.UserRegister
                        @FullName NVARCHAR(100),
                        @Email NVARCHAR(100),
                        @PasswordHash NVARCHAR(MAX)
                    AS
                    BEGIN
                        INSERT INTO Users (FullName, Email, PasswordHash)
                        VALUES (@FullName, @Email, @PasswordHash);
                    END');
                END";
                db.Execute(createRegisterProc);

                string createLoginProc = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID('dbo.UserLogin'))
                BEGIN
                    EXEC('CREATE PROCEDURE dbo.UserLogin
                        @Email NVARCHAR(100)
                    AS
                    BEGIN
                        SELECT * FROM Users WHERE Email = @Email;
                    END');
                END";
                db.Execute(createLoginProc);

                // --- RAPOR PROSEDÜRLERİ ---

                string proc1 = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID('dbo.Rpt_RoomTypeRevenue'))
                BEGIN
                    EXEC('CREATE PROCEDURE dbo.Rpt_RoomTypeRevenue
                    AS
                    BEGIN
                        SELECT rt.Name as RoomTypeName, COUNT(res.Id) as TotalReservations, ISNULL(SUM(res.TotalPrice), 0) as TotalRevenue
                        FROM RoomTypes rt
                        LEFT JOIN Rooms r ON rt.Id = r.RoomTypeId
                        LEFT JOIN Reservations res ON r.Id = res.RoomId
                        GROUP BY rt.Name;
                    END');
                END";
                db.Execute(proc1);

                string proc2 = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID('dbo.Rpt_GuestStays'))
                BEGIN
                    EXEC('CREATE PROCEDURE dbo.Rpt_GuestStays
                    AS
                    BEGIN
                        SELECT g.FirstName, g.LastName, COUNT(res.Id) as TotalStays, ISNULL(SUM(res.TotalPrice), 0) as TotalSpent
                        FROM Guests g
                        INNER JOIN Reservations res ON g.Id = res.GuestId
                        GROUP BY g.FirstName, g.LastName
                        ORDER BY TotalSpent DESC;
                    END');
                END";
                db.Execute(proc2);

                string proc3 = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID('dbo.Rpt_RoomAvailability'))
                BEGIN
                    EXEC('CREATE PROCEDURE dbo.Rpt_RoomAvailability
                    AS
                    BEGIN
                        SELECT rt.Name as RoomTypeName,
                               SUM(CASE WHEN r.IsAvailable = 1 THEN 1 ELSE 0 END) as AvailableCount,
                               SUM(CASE WHEN r.IsAvailable = 0 THEN 1 ELSE 0 END) as OccupiedCount
                        FROM Rooms r
                        INNER JOIN RoomTypes rt ON r.RoomTypeId = rt.Id
                        GROUP BY rt.Name;
                    END');
                END";
                db.Execute(proc3);

                string proc4 = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID('dbo.Rpt_UpcomingReservations'))
                BEGIN
                    EXEC('CREATE PROCEDURE dbo.Rpt_UpcomingReservations
                    AS
                    BEGIN
                        SELECT g.FirstName, g.LastName, r.RoomNumber, res.CheckInDate, res.CheckOutDate
                        FROM Reservations res
                        INNER JOIN Guests g ON res.GuestId = g.Id
                        INNER JOIN Rooms r ON res.RoomId = r.Id
                        WHERE res.CheckInDate >= CAST(GETDATE() AS DATE)
                        ORDER BY res.CheckInDate ASC;
                    END');
                END";
                db.Execute(proc4);

                string proc5 = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID('dbo.Rpt_MonthlyRevenue'))
                BEGIN
                    EXEC('CREATE PROCEDURE dbo.Rpt_MonthlyRevenue
                    AS
                    BEGIN
                        SELECT YEAR(res.CheckInDate) as Year, MONTH(res.CheckInDate) as Month, COUNT(res.Id) as TotalBookings, ISNULL(SUM(res.TotalPrice), 0) as MonthlyRevenue
                        FROM Reservations res
                        GROUP BY YEAR(res.CheckInDate), MONTH(res.CheckInDate)
                        ORDER BY Year DESC, Month DESC;
                    END');
                END";
                db.Execute(proc5);
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
        
        public static T Getir<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();
                return db.QueryFirstOrDefault<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
