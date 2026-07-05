using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HastaneRandevu.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;

namespace HastaneRandevu.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        using (var db = new SqlConnection(Context.connectionstring))
        {
            ViewBag.TotalHasta = db.ExecuteScalar<int>("SELECT COUNT(*) FROM Hasta");
            ViewBag.TotalDoktor = db.ExecuteScalar<int>("SELECT COUNT(*) FROM Doktor");
            ViewBag.TotalPoliklinik = db.ExecuteScalar<int>("SELECT COUNT(*) FROM Poliklinik");
            ViewBag.TodayRandevu = db.ExecuteScalar<int>("SELECT COUNT(*) FROM Randevu WHERE CAST(RandevuTarihi AS DATE) = CAST(GETDATE() AS DATE)");
        }
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
