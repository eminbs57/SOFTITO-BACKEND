using System;

namespace HastaneRandevu.Models
{
    public class RandevuRaporModel
    {
        public int RandevuNo { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public string? Durum { get; set; }
        public string? TCKimlikNo { get; set; }
        public string? HastaAdSoyad { get; set; }
        public string? Cinsiyet { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string? DoktorAdSoyad { get; set; }
        public string? PoliklinikAdi { get; set; }
    }
}
