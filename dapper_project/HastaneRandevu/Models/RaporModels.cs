using System;

namespace HastaneRandevu.Models
{
    public class RaporPoliklinikModel
    {
        public string PoliklinikAdi { get; set; }
        public int ToplamRandevu { get; set; }
    }

    public class RaporDoktorModel
    {
        public string DoktorAdSoyad { get; set; }
        public string PoliklinikAdi { get; set; }
        public int ToplamRandevu { get; set; }
        public int AktifRandevu { get; set; }
        public int TamamlananRandevu { get; set; }
        public int IptalRandevu { get; set; }
    }

    public class RaporAktifHastaModel
    {
        public string HastaAdSoyad { get; set; }
        public int Yas { get; set; }
        public int ToplamRandevu { get; set; }
    }

    public class RaporDurumModel
    {
        public string Durum { get; set; }
        public int ToplamSayi { get; set; }
    }

    public class RaporPopulerDoktorModel
    {
        public string DoktorAdSoyad { get; set; }
        public int ToplamRandevu { get; set; }
    }
}
