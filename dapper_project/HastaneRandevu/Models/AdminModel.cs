using System;

namespace HastaneRandevu.Models
{
    public class AdminModel
    {
        public int AdminID { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
