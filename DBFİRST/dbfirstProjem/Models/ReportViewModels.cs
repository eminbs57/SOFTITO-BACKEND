using System;
using System.Collections.Generic;

namespace dbfirstProjem.Models
{
    public class RaporViewModel
    {
        public List<FilmSatisRaporu> FilmSatisRaporlari { get; set; } = new();
        public List<SeansDolulukRaporu> SeansDolulukRaporlari { get; set; } = new();
        public List<KullaniciHarcamaRaporu> KullaniciHarcamaRaporlari { get; set; } = new();
        public List<TurHasilatRaporu> TurHasilatRaporlari { get; set; } = new();
        public List<SalonSatisRaporu> SalonSatisRaporlari { get; set; } = new();
    }

    // 1. Film Bazlı Satış ve Hasılat Raporu
    public class FilmSatisRaporu
    {
        public string FilmAdi { get; set; } = "";
        public int SatilanBiletSayisi { get; set; }
        public decimal ToplamHasilat { get; set; }
    }

    // 2. Seans Bazlı Doluluk Raporu
    public class SeansDolulukRaporu
    {
        public DateTime TarihSaat { get; set; }
        public string FilmAdi { get; set; } = "";
        public string SalonAdi { get; set; } = "";
        public int SatilanKoltukSayisi { get; set; }
        public int SalonKapasitesi { get; set; }
        public double DolulukOrani => SalonKapasitesi == 0 ? 0 : Math.Round((double)SatilanKoltukSayisi / SalonKapasitesi * 100, 2);
    }

    // 3. Kullanıcı Harcama Raporu
    public class KullaniciHarcamaRaporu
    {
        public string UserId { get; set; } = "";
        public string KullaniciEmail { get; set; } = "";
        public int ToplamBiletSayisi { get; set; }
        public decimal ToplamHarcama { get; set; }
    }

    // 4. Türe Göre Hasılat Raporu
    public class TurHasilatRaporu
    {
        public string FilmTuru { get; set; } = "";
        public decimal ToplamHasilat { get; set; }
    }

    // 5. Salon Bazlı Toplam Satış Raporu
    public class SalonSatisRaporu
    {
        public string SalonAdi { get; set; } = "";
        public int ToplamSatilanBilet { get; set; }
        public decimal ToplamGelir { get; set; }
    }
}
