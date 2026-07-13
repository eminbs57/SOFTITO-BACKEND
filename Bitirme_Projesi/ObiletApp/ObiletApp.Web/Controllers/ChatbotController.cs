using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ObiletApp.Web.Controllers
{
    public class ChatbotRequest
    {
        public string Message { get; set; }
    }

    public class ChatbotController : Controller
    {
        [HttpPost]
        public IActionResult Ask([FromBody] ChatbotRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return Json(new { response = "Lütfen bir mesaj yazın." });
            }

            string input = request.Message.ToLowerInvariant();
            string response = GenerateResponse(input);

            return Json(new { response });
        }

        private string GenerateResponse(string input)
        {

            if (input.Contains("iptal") || input.Contains("iade") || input.Contains("vazgeç"))
            {
                return "Biletinizi iptal etmek veya sorgulamak için ana menüdeki <a href='/Booking/FindTicket' class='text-warning text-decoration-underline'>Bilet Sorgula</a> sayfasına gidebilir ve PNR kodunuzla işleminizi anında gerçekleştirebilirsiniz.";
            }

            if (input.Contains("bagaj") || input.Contains("valiz") || input.Contains("kilo"))
            {
                return "ObiletApp üzerinden alınan otobüs biletlerinde her yolcu için standart bagaj hakkı maksimum 30 KG'dır. Kabin içine sadece küçük el çantanızı (8 KG'a kadar) alabilirsiniz.";
            }

            if (input.Contains("evcil") || input.Contains("hayvan") || input.Contains("kedi") || input.Contains("köpek"))
            {
                return "Aşı karnesi tam olan küçük evcil hayvanlarınızı (kedi, köpek, kuş) standartlara uygun özel taşıma çantalarında (kafeslerinde) olmak şartıyla kabin içinde veya bagaj bölümünde taşıyabilirsiniz. Lütfen sefer öncesi firma ile iletişime geçin.";
            }

            if (input.Contains("nerede") || input.Contains("gecik") || input.Contains("kaldı"))
            {
                return "Seferinizin güncel konumunu ve anlık peron bilgisini öğrenmek için otogardaki ilgili otobüs firmasının yazıhanesiyle iletişime geçmeniz gerekmektedir.";
            }

            if (input.Contains("fiyat") || input.Contains("ücret") || input.Contains("ne kadar"))
            {
                return "Bilet fiyatları güzergaha, seçilen firmaya ve tarihe göre değişiklik göstermektedir. Kesin fiyatları görmek için lütfen ana sayfadan güzergah ve tarih seçip arama yapınız.";
            }

            if (input.Contains("iletişim") || input.Contains("müşteri hizmetleri") || input.Contains("telefon") || input.Contains("şikayet"))
            {
                return "Bize 7/24 ulaşabilirsiniz! Çağrı Merkezi numaramız: 0850 111 22 33 veya destek@obiletapp.com adresine e-posta gönderebilirsiniz.";
            }

            if (input.Contains("merhaba") || input.Contains("selam"))
            {
                return "Merhaba! Sana nasıl yardımcı olabilirim? Bilet iptali, bagaj hakkı veya başka konularda soru sorabilirsin.";
            }
            if (input.Contains("teşekkür") || input.Contains("sağol"))
            {
                return "Ben teşekkür ederim! Sana yardımcı olmak benim görevim. Başka bir sorun olursa her zaman buradayım. İyi yolculuklar! 🚌";
            }
            if (input.Contains("nasılsın"))
            {
                return "Ben harikayım, saniyede binlerce işlemi yapabilecek güce sahibim! Sen nasılsın? Yolculuk planı yapıyorsan sana yardımcı olmaya hazırım.";
            }

            return "Anlayamadım 😔. Ben ObiletApp'in yapay zeka asistanıyım. Henüz öğrenme aşamasındayım. 'Bilet iptali', 'Bagaj hakkı', 'Evcil hayvan' veya 'İletişim' gibi kelimeler kullanarak sorunu daha basit sorarsan sevinirim!";
        }
    }
}
