using System;

namespace HastaneRandevu.Models
{
    public class RandevuModel
    {
        public int RandevuNo { get; set; }
        public int HastaNo { get; set; }
        public int DoktorNo { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public string Durum { get; set; }
    }
}