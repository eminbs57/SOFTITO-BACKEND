B2B E-Ticaret Yönetim Paneli

İşletmelerin ürün stoklarını, siparişlerini, müşterilerini ve satış raporlarını tek bir merkezden yönetmelerini sağlayan, ASP.NET Core MVC mimarisi ile geliştirilmiş dinamik bir E-Ticaret Admin Dashboard projesidir. Sistem; oturum ve çerez (cookie) yönetimi, yetkilendirme ve gelişmiş güvenlik katmanları ile profesyonel bir B2B altyapısı sunar.

🚀 Temel Özellikler

Güvenli Kimlik Yönetimi: ASP.NET Core Identity ile kapsamlı kullanıcı kaydı, giriş/çıkış işlemleri ve rol tabanlı yetkilendirme.

Veri Yönetimi: PDF ve Excel formatında raporlama ve dışa aktarma (export) desteği.

İlişkisel Veritabanı Yönetimi: LINQ ve Entity Framework Core kullanarak karmaşık JOIN operasyonları ve performanslı veri sorgulama mimarisi.

Performans ve Optimizasyon: Gelişmiş cacheleme mekanizmaları ile hızlı veri erişimi.

İzlenebilirlik: Sistem üzerindeki kritik işlemlerin takibi için merkezi loglama altyapısı.

💻 Kullanılan Teknolojiler

Framework: C# .NET Core 8.0 (MVC)

ORM: Entity Framework Core (Code-First Approach)

Veritabanı: MS SQL Server (Docker Container üzerinden)

Güvenlik: ASP.NET Core Identity (Şifre hashleme, oturum yönetimi)

## 📸 Ekran Görüntüleri 

<div align="center">
  <img src="projectcodefrst/screenshots/Dashboard.png" alt="Dashboard Görünümü" width="800"/>
  <br/>
  <i>Modern Yönetici Paneli ve İstatistikler (Dashboard)</i>
  <br/><br/>

  <img src="projectcodefrst/screenshots/login.png" alt="Giriş Sayfası" width="800"/>
  <br/>
  <i>Güvenli Kullanıcı Giriş Ekranı (Login)</i>
  <br/><br/>

  <img src="projectcodefrst/screenshots/prdouct.png" alt="Ürün Yönetimi" width="800"/>
  <br/>
  <i>Katalog ve Ürün Yönetim Modülü</i>
  <br/><br/>

  <img src="projectcodefrst/screenshots/order.png" alt="Siparişler" width="800"/>
  <br/>
  <i>Sipariş Takip ve Yönetim Ekranı</i>
  <br/><br/>

  <img src="projectcodefrst/screenshots/report.png" alt="Raporlama" width="800"/>
  <br/>
  <i>Gelişmiş Satış ve Stok Raporları</i>
</div>

> 💡 **Not:** Projeye ait diğer tüm detaylı ekran görüntülerine yukarıdaki dosya listesinden `screenshots` klasörüne tıklayarak erişebilirsiniz.
## 🚀 Adım Adım Nasıl Çalıştırılır?

Bu proje **Code-First** (Önce Kod) yaklaşımı ile geliştirilmiştir. Projeyi sorunsuz çalıştırmak için aşağıdaki adımları sırasıyla izleyin:

1. **Veritabanı Bağlantısını Ayarlama:**
   - Proje içerisindeki `codefirstproject/projectcodefrst/appsettings.json` dosyasını açın.
   - `ConnectionStrings` altındaki bağlantı dizesini kendi lokal SQL Server'ınıza uyacak şekilde düzenleyin.

2. **Migration ve Veritabanı Oluşturma:**
   - Ana depo kök dizininde (`SOFTITO-BACKEND` klasörü içinde) bir terminal açın.
   - Veritabanını koddan türetmek için migration'ları uygulayın:
   ```bash
   dotnet ef database update --project codefirstproject/projectcodefrst/projectcodefrst.csproj
   ```

3. **Projeyi Başlatma:**
   - Veritabanı oluştuktan sonra uygulamayı çalıştırın:
   ```bash
   dotnet run --project codefirstproject/projectcodefrst/projectcodefrst.csproj
   ```

4. **Kullanım:**
   - Tarayıcınızda açılan adres üzerinden uygulamayı test edebilirsiniz.