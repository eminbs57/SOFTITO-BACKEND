<div align="center">
  <img src="https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white" alt=".NET" />
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/Microsoft_SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server" />
  <img src="https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=Swagger&logoColor=black" alt="Swagger" />
</div>

<br />

<div align="center">
  <h1 align="center">🚌 ObiletApp - Yeni Nesil Otobüs/Uçak Bileti Rezervasyon ve Yönetim Sistemi</h1>
  <p align="center">
    <strong>Clean Architecture</strong> ve <strong>CQRS</strong> prensipleriyle inşa edilmiş, yüksek performanslı ve tam donanımlı kurumsal B2B/B2C çözümü.
    <br />
    <br />
    <a href="#-proje-hakkında">Proje Hakkında</a>
    ·
    <a href="#-mimari-yapı-clean-architecture">Mimari Yapı</a>
    ·
    <a href="#-kapsamlı-özellikler">Özellikler</a>
    ·
    <a href="#-kurulum-ve-kullanım">Kurulum</a>
  </p>
</div>

---

## 📖 Proje Hakkında

**ObiletApp**, otobüs/uçak
firmaları ve yolcuları dijital bir platformda buluşturan, uçtan uca yönetilebilir bir bilet rezervasyon sistemidir. Günümüz modern yazılım mühendisliği standartlarına uygun olarak tasarlanan bu proje, sadece çalışan bir web sitesi olmakla kalmaz; arka planda mikroservis mantığına uygun, genişletilebilir ve sağlam bir altyapı barındırır.

Kullanıcılar sisteme girip seferleri filtreleyebilir, dinamik koltuk haritası üzerinden istedikleri koltuğu seçip güvenle satın alabilirler. Bilet işlemi sonrasında oluşan PNR kodu ve özel olarak üretilen **QR Kod** sayesinde dijital biletlerini her an yanlarında taşıyabilirler. Sistem yöneticileri ise gelişmiş Admin Paneli sayesinde firmaları, güzergahları, seferleri ve kampanyaları yönetirken, sistemin ürettiği karmaşık finansal verileri **Excel** ve **PDF** raporlarına dönüştürebilirler.

Sistem, yapay zeka destekli bir **NLP Akıllı Asistan (Chatbot)** barındırarak kullanıcı deneyimini bir üst seviyeye taşır.

---

## 🏗 Mimari Yapı (Clean Architecture)

Aşağıdaki şema, sistemin arka planında çalışan modüler, genişletilebilir ve güvenlik odaklı kurumsal mimariyi özetlemektedir:

```mermaid
graph TD
    %% Katmanlar
    subgraph ClientLayer [Client Layer]
        UI[ObiletApp.Web<br/>MVC UI & Dark Theme]
    end

    subgraph APILayer [API Layer]
        API[ObiletApp.API<br/>RESTful API & JWT Auth]
    end

    subgraph InfrastructureLayer [Infrastructure Layer]
        EF[ObiletApp.Infrastructure<br/>EF Core - Write Operations]
        DAP[Dapper Repositories<br/>Fast Read & Reports]
        Chatbot[AI & Shared Services<br/>NLP Chatbot]
    end

    subgraph ApplicationCore [Application Core Layer]
        APP[ObiletApp.Application<br/>Interfaces, DTOs, CQRS Services]
        DOM[ObiletApp.Core<br/>Domain Entities, Enums]
    end

    DB[(Microsoft SQL Server 2022)]

    %% İlişkiler
    UI -- "HTTP Requests / JSON" --> API
    UI -- "Direct Service Call (Optional)" --> APP
    API -- "Uses Services" --> APP
    
    EF -- "Implements" --> APP
    DAP -- "Implements" --> APP
    Chatbot -- "Implements" --> APP
    
    APP -- "Depends On" --> DOM
    
    EF -- "Write (Insert/Update/Delete)" --> DB
    DAP -- "Fast Read (Complex JOINs)" --> DB
    
    %% Stillendirme
    classDef layerStyle fill:#212529,stroke:#6c757d,stroke-width:2px,color:#fff;
    classDef dbStyle fill:#198754,stroke:#146c43,stroke-width:2px,color:#fff;
    classDef default fill:#343a40,stroke:#6c757d,color:#f8f9fa;
    
    class UI,API,EF,DAP,Chatbot,APP,DOM layerStyle;
    class DB dbStyle;
```

Proje sürdürülebilirliği artırmak, bağımlılıkları (coupling) azaltmak ve test edilebilirliği en üst düzeye çıkarmak için **Onion/Clean Architecture** tasarım deseni kullanılarak 4 ana katmana ayrılmıştır:

1. **ObiletApp.Core (Domain Layer):** 
   Sistemin kalbidir. Hiçbir dış kütüphaneye bağımlı değildir. Bütün `Entity` (Varlık) sınıfları (Örn: *Ticket, Passenger, Company, Vehicle*) ve temel arayüzler (Interfaces) burada yer alır.
2. **ObiletApp.Application (Use Cases Layer):**
   İş kurallarının (Business Logic) işlendiği yerdir. DTO'lar, Mapping işlemleri, CQRS (Command Query Responsibility Segregation) pattern ile tasarlanmış servisler burada tutulur. 
3. **ObiletApp.Infrastructure (Data & Cross-Cutting Layer):**
   Veritabanı işlemleri ve dış servis bağlantılarının bulunduğu katmandır. **Repository Pattern** kullanılarak veritabanı soyutlanmıştır. Write (Yazma/Değiştirme) işlemleri için güvenliğiyle öne çıkan **Entity Framework Core (Code-First)**, karmaşık Read (Okuma/Raporlama) işlemleri için ise maksimum performans sağlayan mikro-ORM **Dapper** eşzamanlı olarak kullanılmıştır.
4. **ObiletApp.Web & ObiletApp.API (Presentation Layer):**
   Kullanıcının ve diğer sistemlerin etkileşime geçtiği yüzdür. Web tarafı **ASP.NET Core MVC** ile geliştirilmiş olup Bootstrap 5 ile şekillendirilmiştir. API tarafı ise mobil uygulamalar veya dış servisler için tasarlanmış, **JWT Token** ile korunan tamamen RESTful bir altyapıya sahiptir.




---

## 📂 Proje Klasör Yapısı (Solution Tree)

Aşağıdaki yapı, **Clean Architecture** prensiplerinin klasör düzeyinde nasıl titizlikle uygulandığını göstermektedir:

```text
ObiletApp/
├── ObiletApp.Core/                  # [Domain Layer] Dışa bağımlılığı SIFIR olan çekirdek
│   ├── Entities/                    # Veritabanı tablolarının C# karşılıkları (Vehicle, Ticket vb.)
│   ├── Enums/                       # Cinsiyet, Bilet Durumu gibi sabitler
│   └── Interfaces/                  # IRepository gibi temel soyutlamalar
│
├── ObiletApp.Application/           # [Use Cases Layer] İş Kuralları
│   ├── DTOs/                        # Veri transfer objeleri (Frontend'e giden hafifleştirilmiş veriler)
│   ├── Mapping/                     # Entity -> DTO dönüşümleri (AutoMapper)
│   └── Services/                    # Gerçek iş mantığının (Business Logic) yazıldığı yer
│
├── ObiletApp.Infrastructure/        # [Data Layer] Veritabanı ve Dış Servisler
│   ├── Data/                        # ApplicationDbContext (EF Core) konfigürasyonları
│   ├── Repositories/                # EF Core (Generic) ve Dapper için Repository pattern uygulamaları
│   └── Migrations/                  # Veritabanı versiyon kontrolü
│
├── ObiletApp.API/                   # [Presentation - 1] RESTful Servisler (EF Core)
│   ├── Controllers/                 # Swagger ile test edilebilen, JWT korumalı endpointler
│   └── Program.cs                   # API ayağa kalkış ayarları ve DI Container
│
├── ObiletApp.API.Dapper/            # [Presentation - 2] Dapper Odaklı Mikro API
│   ├── Controllers/                 # Performans odaklı, saf okuma (Read) işlemlerinin sunulduğu API uçları
│   └── Program.cs                   # Dapper bağlantılarının ve DI Container'ın ayarlandığı yapı
│
└── ObiletApp.Web/                   # [Presentation - 3] MVC Kullanıcı Arayüzü
    ├── Controllers/                 # Admin ve Müşteri sayfalarının yönlendiricileri
    ├── Views/                       # HTML/CSS/JS/Razor dosyaları (UI)
    └── wwwroot/                     # Statik dosyalar, Bootstrap, JS scriptleri
```

---

## 🔄 Bilet Satın Alma İş Akışı (Sequence Diagram)

Kullanıcının koltuk seçip bileti satın alana kadar arka planda sistemin nasıl haberleştiğini gösteren iş akışı:

```mermaid
sequenceDiagram
    actor Müşteri
    participant WebUI as MVC Web Arayüzü
    participant API as RESTful API (JWT Korumalı)
    participant App as Application Layer (CQRS/Services)
    participant EF as EF Core (Veritabanı Yazma)
    participant Dapper as Dapper (Veritabanı Okuma)

    Müşteri->>WebUI: Sefer arar ve listeler
    WebUI->>Dapper: Hızlı okuma ile seferleri getir (JOINs)
    Dapper-->>WebUI: Sefer listesi döndürür
    Müşteri->>WebUI: Koltuk seçer ve satın al butonuna tıklar
    WebUI->>API: HTTP POST /api/tickets (Satın Alma İsteği)
    API->>App: TicketCreateCommand tetiklenir
    App->>App: İş Kuralları: Koltuk boş mu? Fiyat doğru mu?
    App->>EF: Yeni bileti veritabanına kaydet
    EF-->>App: İşlem Başarılı (PNR Kodu üretildi)
    App->>App: QR Kod Üretici (QRCoder) çalışır
    App-->>API: Başarı Yanıtı + PNR + QR Kod
    API-->>WebUI: 200 OK
    WebUI-->>Müşteri: Biletiniz başarıyla oluşturuldu! (SweetAlert2)
```

---

## 💡 Kullanılan Tasarım Desenleri (Design Patterns)

Projenin sürdürülebilirliği ve kod kalitesi için aşağıdaki yazılım tasarım desenleri ve prensipleri titizlikle uygulanmıştır:

- **Repository Pattern & Unit of Work:** Veritabanı işlemleri (EF Core ve Dapper) soyutlanarak, iş katmanının (Application) doğrudan veritabanına bağımlı olması engellenmiştir. Değişiklikler Unit of Work ile tek bir transaction üzerinden güvenle yönetilir.
- **CQRS (Command Query Responsibility Segregation):** Sistemi yormamak adına "Okuma" (Query) işlemleri Dapper ile yüksek performansla yapılırken, "Yazma/Güncelleme" (Command) işlemleri EF Core'un veri bütünlüğü güvenliğiyle sağlanmıştır.
- **Dependency Injection (DI):** Tüm servisler ve arayüzler `Program.cs` içerisinde IoC Container'a kaydedilmiş olup, nesne oluşturma maliyetleri azaltılmış ve bağımlılıklar gevşek (loosely coupled) hale getirilmiştir.
- **Singleton Pattern:** Uygulama genelinde tek bir örnekte çalışması gereken yardımcı servisler (Örneğin; sistem loglayıcılar veya yapay zeka yapılandırmaları) için kullanılmıştır.

---

## ⚡ Örnek API İstek ve Yanıtları

Sistemin API mimarisinin ne kadar temiz ve standartlara uygun çalıştığını gösteren örnek bir uç nokta (Endpoint):

**İstek (Request):** Seçili seferin detaylarını getiren API çağrısı
```http
GET /api/Voyages/5 HTTP/1.1
Host: localhost:5121
Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn... (JWT Token)
```

**Yanıt (Response):** Dapper tarafından milisaniyeler içinde üretilen ilişkisel JSON dönüşü
```json
{
  "success": true,
  "data": {
    "voyageId": 5,
    "departureCity": "İstanbul",
    "arrivalCity": "Ankara",
    "departureTime": "2026-08-15T14:30:00Z",
    "price": 450.00,
    "vehicle": {
      "plateNumber": "34 ABC 123",
      "capacity": 40,
      "hasWifi": true
    },
    "companyName": "Kamil Koç",
    "availableSeats": 24
  },
  "message": "Sefer bilgileri başarıyla getirildi."
}
```

---

## 🔒 Güvenlik Mimarisi (Security Strategy)

Sistemin bütünlüğü ve kullanıcı verilerinin gizliliği için çok katmanlı bir güvenlik yapısı kurgulanmıştır:

- **Şifreleme:** Kullanıcı parolaları veritabanında asla düz metin (plain-text) olarak tutulmaz. **ASP.NET Core Identity** altyapısı kullanılarak PBKDF2 algoritması ile dönüştürülmüş Hash'ler olarak (PasswordHash) saklanır.
- **Kimlik Doğrulama (Authentication):** Web arayüzünde güvenli Cookie (Çerez) tabanlı oturum yönetimi sağlanırken, dışa açık API uç noktaları endüstri standardı olan **JWT (JSON Web Token)** ile korunmaktadır. API'ye yapılan izinsiz istekler (401 Unauthorized) anında reddedilir.
- **Yetkilendirme (Authorization):** Rol bazlı (Role-Based) yetkilendirme sistemi sayesinde sıradan kullanıcılar Admin paneline ve kritik API uç noktalarına (Örn: Sefer İptali, Rapor Görüntüleme) erişemez (403 Forbidden).

---

## ⚡ Performans ve Optimizasyon Kararları

Projede yüksek trafiği kaldırabilecek, bekleme sürelerini minimize eden mühendislik tercihleri yapılmıştır:

- **Çift ORM Stratejisi (CQRS Temelli):**
  - "Okuma" operasyonlarının (Örn: Ana sayfada binlerce seferi filtreleme, Rapor çekme) milisaniyeler sürmesi gerektiği için nesne takibi (Tracking) yapmayan, saf ADO.NET hızında çalışan mikro-ORM **Dapper** kullanılmıştır. Performansı boğan karmaşık LINQ sorguları yerine optimize edilmiş SQL (JOIN, GROUP BY) sorguları yazılmıştır.
  - "Yazma" operasyonları (Bilet Alma, Kullanıcı Kaydı) ise veri bütünlüğünün ve validasyon kurallarının kusursuz işlemesi için **Entity Framework Core** ile gerçekleştirilmiştir. Böylece hız ve güvenlik kusursuz bir dengeye oturtulmuştur.
- **Veritabanı Normalizasyonu:** Tablolar (Vehicle, Route, Voyage, Ticket vb.) 3. Normal Form'a (3NF) uygun tasarlanarak veri tekrarı önlenmiş ve veritabanı boyutunun gereksiz şişmesinin önüne geçilmiştir.

---

## 🚀 Kapsamlı Özellikler

### 👤 Müşteri (B2C) Özellikleri
- **Kapsamlı Arama & Dinamik Filtreleme:** Kullanıcılar kalkış ve varış noktalarını, tarihleri girerek aktif seferleri listeleyebilir; saat, firma ve fiyata göre anlık filtreleme yapabilir.
- **Canlı Koltuk Seçimi:** Araç kapasitesine göre dinamik çizilen koltuk haritası üzerinden boş koltukların (Kadın/Erkek yan yana oturma kurallarına göre) seçilmesi.
- **Akıllı Asistan (Chatbot):** NLP mantığı ile çalışan entegre yapay zeka botu. Kullanıcılar *"Bagaj hakkım nedir?", "Biletimi nasıl iptal edebilirim?"* gibi sorular sorduğunda anında doğru ve yönlendirici cevaplar alır.
- **PNR Kodu ve QR Kod:** Bilet alındığında sistem otomatik olarak benzersiz bir PNR üretir. Kullanıcı ekranında oluşan **QR Kod** sayesinde bilet dijital olarak doğrulanabilir.
- **Bilet İptal & Sorgulama:** Kullanıcılar üye dahi olmadan sadece PNR numarası ile bilet detaylarına ulaşabilir veya sefer saatine belli bir süre kalana dek biletlerini iptal edebilirler.

### 🛡 Admin ve Yönetim (B2B) Özellikleri
- **Rol Tabanlı Kimlik Doğrulama:** ASP.NET Core Identity altyapısı kullanılarak SuperAdmin, Firma Yöneticisi gibi farklı rol hiyerarşileri oluşturulmuştur.
- **Gelişmiş CRUD ve İlişkisel Yönetim:** 
  - **Araçlar & Filo:** Araçların plaka, koltuk kapasitesi ve Wi-fi/TV özelliklerinin yönetimi.
  - **Firmalar & Güzergahlar:** Platformda bilet satan firmaların (Kamil Koç, Pamukkale vb.) ve sefer yapılan şehir/lokasyon verilerinin yönetimi.
  - **Kampanyalar:** Dinamik kampanya afişleri ve indirim kodlarının sisteme girilmesi.
- **Dapper Destekli Detaylı Raporlar:** Admin paneli, klasik Entity Framework'ün yavaş kalabileceği çoklu tablo (JOIN) sorgularını saf SQL gücüyle **Dapper** kullanarak çeker. "Aylık en çok bilet satan firma", "Doluluk oranı en yüksek sefer" gibi analizler saniyeler içinde sunulur.
- **Excel & PDF Dışa Aktarma:** Yöneticiler, oluşturdukları raporları tek tıkla **ClosedXML** kütüphanesi sayesinde Excel dosyası olarak indirebilir, muhasebeye sunabilir.

### 🔌 Teknik & Sistem Özellikleri
- **Güvenli RESTful API:** Tüm sistem fonksiyonları dış sistemlere (Örn: iOS/Android app) açılmak üzere API katmanında yazılmıştır. Endpoints'ler izinsiz girişleri önlemek için **JWT (JSON Web Token)** kullanılarak şifrelenmiştir.
- **Swagger Dokümantasyonu:** API mimarisinin tamamı Swagger UI ile belgelenmiş olup, front-end veya mobil geliştiricilerin API'yi zahmetsizce test etmesi sağlanmıştır.
- **SweetAlert2 ve Asenkron UI:** Sistemdeki ekleme, silme, bilet satın alma gibi kritik uyarılarda standart Javascript alertleri yerine kullanıcı dostu modern pop-up'lar (SweetAlert2) kullanılmıştır. AJAX tabanlı asenkron yüklemelerle sayfa yenilenmeden işlem yapılır.

---

## 📸 Ekran Görüntüleri ve Arayüz

### Kullanıcı Deneyimi (Bilet Alım Süreci)
| Ana Sayfa | Sefer Sonuçları ve Filtreleme |
| :---: | :---: |
| ![Ana Sayfa](screenshots/user-anaSayfa.png) | ![Seferler](screenshots/seferrSonuclari.png) |

| Koltuk Seçimi | Bilet İptal / Sorgulama |
| :---: | :---: |
| ![Koltuk Sec](screenshots/user-koltukSec.png) | ![Bilet Iptal](screenshots/user-biletIptal.png) |

| Akıllı Asistan (Chatbot) | Kullanıcı Giriş |
| :---: | :---: |
| ![Chatbot](screenshots/chatbot1.png) | ![Login](screenshots/user-login.png) |

### Admin Yönetim Paneli
| Dashboard ve Analitik | Gelişmiş Raporlar (Dapper) |
| :---: | :---: |
| ![Dashboard](screenshots/dashboard.png) | ![Rapor](screenshots/rapor8.png) |

| Sefer Yönetimi | Dinamik Arama (Search) |
| :---: | :---: |
| ![Firma](screenshots/Sefer.png) | ![Search](screenshots/firmaSearch.png) |

### API ve Dokümantasyon
| Genel API Yapısı | Dapper Güçlü Endpointler |
| :---: | :---: |
| ![Swagger API](screenshots/Swagger_Api.png) | ![Swagger Dapper](screenshots/Swagger_ApiDapper.png) |

> 💡 **Not:** Projeye ait diğer tüm detaylı ekran görüntülerine yukarıdaki dosya listesinden `screenshots` klasörüne tıklayarak erişebilirsiniz.

---

## 💻 Kurulum ve Çalıştırma Adımları

Bu projeyi bilgisayarınızda (localhost) çalıştırmak için aşağıdaki adımları sırasıyla uygulayabilirsiniz:

### Gereksinimler
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (veya en güncel sürüm)
- Microsoft SQL Server (LocalDB veya SSMS üzerinden uzak sunucu)
- Visual Studio 2022 veya Visual Studio Code

### Adım Adım Kurulum

1. **Projeyi Klonlayın:**
   ```bash
   git clone https://github.com/eminbs57/SOFTITO-BACKEND.git
   cd SOFTITO-BACKEND/Bitirme_Projesi/ObiletApp
   ```

2. **Veritabanı Bağlantısını (Connection String) Güncelleyin:**
   Kök dizindeki `ObiletApp.Web/appsettings.json` ve `ObiletApp.API/appsettings.json` dosyalarını açıp, SQL Server bağlantı dizenizi kendi yerel veritabanı adınıza göre güncelleyin.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ObiletAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. **Veritabanını ve Tabloları Oluşturun (Migrations):**
   Projeyi ayağa kaldırmadan önce Code-First mimarisindeki veritabanı tablolarını SQL sunucusuna basmalısınız. Terminalden aşağıdaki komutu çalıştırın:
   ```bash
   dotnet ef database update --project ObiletApp.Infrastructure --startup-project ObiletApp.Web
   ```

4. **Projeyi Başlatın:**
   Her şey hazır. MVC Web projesini ayağa kaldırmak için:
   ```bash
   dotnet run --project ObiletApp.Web
   ```

5. **Test Edin:**
   - Web arayüzü: `https://localhost:5120` (veya konsolda belirtilen port)
   - API Dokümantasyonu (Swagger): `https://localhost:5121/swagger`

---

## 👨‍💻 Geliştirici ve İletişim

**Muhammet Emin Baş**  
Bu proje, Softito eğitim programı kapsamında geliştirilmiş bir **Mezuniyet / Bitirme Projesidir**. Modern yazılım mühendisliği prensiplerini gerçek dünya senaryolarına (Bilet Rezervasyon Sistemi) uygulamak amacıyla titizlikle kodlanmıştır.
