# Event Management System (Etkinlik Yönetim Sistemi)

<div align="center">
  <img src="EventManagement/screenshots/user-etkinlikleriKesfet.png" alt="Etkinlik Yönetim Sistemi" width="800"/>
</div>

Modern, dinamik ve kullanıcı dostu bir arayüze sahip olan bu proje; **ASP.NET Core MVC**, **Entity Framework Core**, **ASP.NET Core Identity** ve **jQuery/AJAX** teknolojileri kullanılarak geliştirilmiş kapsamlı bir Etkinlik Yönetim Sistemidir.

## 🚀 Proje Özellikleri

### 👥 Rol Bazlı Yetkilendirme (ASP.NET Core Identity)

Sistem **Admin** ve **User** olmak üzere iki ana yetki seviyesine sahiptir:

- **Admin:** Kapsamlı CRUD işlemleri, etkinlik yönetimi, kategori kontrolü ve detaylı dashboard erişimi.
- **User:** Etkinlikleri inceleme, kontenjan durumuna göre katılım sağlama ve "Etkinliklerim" sayfasından dijital bilet yönetimi.

### 💻 Teknolojiler ve Altyapı

- **Backend:** ASP.NET Core 10.0 MVC
- **ORM:** Entity Framework Core (Code First)
- **Kimlik Doğrulama:** ASP.NET Core Identity
- **Asenkron İletişim:** AJAX & jQuery (Sayfa yenilemeden veri operasyonları)
- **Loglama:** Serilog (Detaylı günlük hata ve işlem takibi)
- **Tasarım:** Bootstrap tabanlı responsive UI, dinamik modal yapıları

### ✨ Öne Çıkan Fonksiyonlar

- **Dinamik Modal Yönetimi:** Tüm CRUD işlemleri, sayfayı yenilemeden AJAX/jQuery destekli pop-up modallar üzerinden yürütülür.
- **Akıllı Kontenjan Takibi:** Etkinlik kapasiteleri veritabanı üzerinden anlık izlenir; dolu etkinlikler için katılım otomatik olarak kısıtlanır.
- **Kurumsal Raporlama:** İstatistiksel veriler (toplam etkinlik/kategori) anında dışa aktarılabilir formatta sunulur.
- **Dinamik Medya Yönetimi:** Admin panelinden yüklenen etkinlik görselleri kullanıcı arayüzüne otomatik entegre edilir.

## 📸 Ekran Görüntüleri

<div align="center">
  
  <img src="EventManagement/screenshots/dashboard.png" alt="Admin Panel" width="800"/>
  <br/><i>Admin Dashboard ve Raporlama</i><br/><br/>

  <img src="EventManagement/screenshots/etkinlikler.png" alt="Etkinlik Listesi" width="800"/>
  <br/><i>Etkinlikler (crud)</i><br/><br/>

  <img src="EventManagement/screenshots/kategoriler.png" alt="Kategoriler" width="800"/>
  <br/><i>Kategoriler (crud)</i><br/><br/>

  <img src="EventManagement/screenshots/user-etkinliklerim.png" alt="Etkinliklerim" width="800"/>
  <br/><i>Kullanıcıya özel katıldığı etkinlikler görüntülemeleri</i><br/><br/>
</div>


> 💡 **Not:** Projeye ait diğer tüm detaylı ekran görüntülerine yukarıdaki dosya listesinden `screenshots` klasörüne tıklayarak erişebilirsiniz.

## 🚀 Adım Adım Nasıl Çalıştırılır?

Bu proje, istemci tarafı AJAX işlemleri için **jQuery** kullanılarak geliştirilmiştir.

1. **Veritabanı Ayarları (Eğer varsa):**
   - Proje veritabanı bağlantısı gerektiriyorsa `jqueryproject/EventManagement/appsettings.json` dosyasını güncelleyin ve gerekiyorsa migration adımlarını tamamlayın.

2. **Projeyi Başlatma:**
   - Ana depo kök dizininde (`SOFTITO-BACKEND`) bir terminal açın:
   ```bash
   dotnet run --project jqueryproject/EventManagement/EventManagement.csproj
   ```

3. **Kullanım:**
   - Proje ayağa kalktıktan sonra tarayıcı üzerinden jQuery AJAX isteklerinin arka uç (.NET) ile nasıl etkileşime girdiğini test edebilirsiniz.
