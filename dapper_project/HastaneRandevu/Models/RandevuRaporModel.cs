using System;

namespace HastaneRandevu.Models
{
    public class RandevuRaporModel
    {
        public string DoktorAdSoyad { get; set; }
        public string PoliklinikAdi { get; set; }
        public int ToplamRandevu { get; set; }
        public int AktifRandevu { get; set; }
        public int TamamlananRandevu { get; set; }
        public int IptalRandevu { get; set; }
    }
}
