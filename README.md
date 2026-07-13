# Softito Backend Projeleri

Bu depo, Softito eğitimi kapsamında geliştirilen 10 farklı .NET Backend projesini içermektedir. Projeler, C# ve .NET ekosistemindeki farklı mimari yaklaşımları, ORM araçlarını ve web teknolojilerini öğrenmek ve uygulamak amacıyla geliştirilmiştir.

## 📂 Projeler

Aşağıda depoda bulunan projelerin kısa açıklamalarını bulabilirsiniz:

1. **[codefirstproject (Code First)](./codefirstproject/)**
   - Entity Framework "Code First" (Önce Kod) yaklaşımının uygulandığı proje. Veritabanı yapısı doğrudan C# sınıfları yazılarak oluşturulmuştur.

2. **[DBFİRST (Database First)](./DBFİRST/)**
   - Entity Framework "Database First" (Önce Veritabanı) yaklaşımının uygulandığı proje. Mevcut bir veritabanından veri modelleri otomatik olarak oluşturulmuştur.

3. **[KatmanlıProje (N-Tier Architecture)](./KatmanlıProje/)**
   - N-Katmanlı mimari (Core, Data, UI) kullanılarak geliştirilmiş kapsamlı bir Kütüphane Yönetim Sistemi.
   - **Kullanılan Teknolojiler:** ASP.NET Core MVC, Entity Framework Core, ASP.NET Core Identity, ClosedXML (Excel raporlama) ve QuestPDF (PDF raporlama).

4. **[RazorPagesProject](./RazorPagesProject/)**
   - Web arayüzleri oluşturmak için MVC'ye güçlü bir alternatif olan, sayfa odaklı (page-focused) ASP.NET Core Razor Pages framework'ünün kullanıldığı proje.

5. **[jqueryproject](./jqueryproject/)**
   - İstemci tarafı işlemleri ve arka uçla asenkron veri iletişimi (AJAX) için .NET altyapısı ile **jQuery** kütüphanesinin entegrasyonunu gösteren proje.

6. **[API-MVC](./API-MVC/)**
   - Bir uygulamanın bağımsız bir API arka ucu (`ApiProject`) ve bu API'yi tüketen bir MVC ön yüz istemcisi (`MvcProject`) olarak ikiye ayrıldığı proje. Sorumlulukların ayrılması (separation of concerns) prensibini temel alır.

7. **[dapper_project](./dapper_project/)**
   - Dapper mikro-ORM'inin temel kullanımını ve veritabanı üzerinde hızlı CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemlerini gösteren proje.

8. **[ApiDapper-mvc](./ApiDapper-mvc/)**
   - Veritabanı işlemleri için Entity Framework yerine hızlı ve hafif bir mikro-ORM olan **Dapper**'ın kullanıldığı ve MVC mimarisiyle entegre edildiği proje.

9. **[PetAdoptionORM](./PetAdoptionORM/)**
   - Evcil hayvan sahiplenme domain'i (alanı) üzerine odaklanmış, Code First ORM (Nesne-İlişkisel Eşleme) yaklaşımını kullanan uygulamalı bir proje.

10. **[Bitirme_Projesi (ObiletApp)](./Bitirme_Projesi/)**
    - Otobüs/Uçak bileti rezervasyon ve yönetim sistemi. Clean Architecture, Repository Pattern ve CQRS yaklaşımları harmanlanmış; okuma işlemleri için Dapper, yazma işlemleri için EF Core kullanılmış kapsamlı B2B/B2C bitirme projesi. Yapay Zeka destekli Chatbot içerir.


## 🚀 Temel Teknolojiler ve Araçlar

- **Dil & Platform:** C#, .NET / .NET Core
- **Web Çatıları:** ASP.NET Core MVC, ASP.NET Core Web API, ASP.NET Core Razor Pages
- **ORM & Veri Erişimi:** Entity Framework Core, Dapper
- **Veritabanı Yaklaşımları:** Code First, Database First
- **Kimlik Doğrulama:** ASP.NET Core Identity
- **Veritabanı:** Microsoft SQL Server
- **Frontend Entegrasyonları:** jQuery, Bootstrap, HTML/CSS
