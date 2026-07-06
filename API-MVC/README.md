# (API-MVC) Lojistik Yönetim Sistemi

Bu proje, bir lojistik/filo yönetim sisteminin temelini oluşturan; RESTful API servisi ve kapsamlı bir Web yönetim panelinden oluşan bütünleşik bir çözümdür.

## 🏗 Mimari Yapı

Proje, `ApiMvcProject.slnx` çözüm dosyası altında iki ana katmanlı yapıda geliştirilmiştir:

### 1. ApiProject (RESTful Web API)

Sistemin veri sağlayan ve işleyen arka uç (backend) servisidir.

- **Teknolojiler:** ASP.NET Core Web API, Entity Framework Core
- **Veritabanı:** SQL Server (`ApiDatabase`)
- **Modeller:** `Vehicle` (Araç), `Driver` (Sürücü), `Depo` (Depo)
- **Özellikler:** Swagger/OpenAPI desteği ile standartlara uygun uç noktalar.

### 2. MvcProject (Web Uygulaması)

Lojistik varlıkların yönetildiği, kullanıcı doğrulama ve yetkilendirme süreçlerini barındıran yönetim panelidir.

- **Teknolojiler:** ASP.NET Core MVC, Entity Framework Core, Cookie Authentication
- **Veritabanı:** SQL Server (`MvcDatabase`)
- **Modeller:**
  - _Lojistik:_ `Vehicle`, `Driver`, `Depo`
  - _Kullanıcı:_ `User`, `Role`, `Department`
- **Özellikler:** Rol/Departman tabanlı yetkilendirme, güvenli oturum yönetimi ve tam kapsamlı CRUD işlemleri.

## 📸 Ekran Görüntüleri

<div align="center">
  <img src="screenshots/dashboard.png" alt="Dashboard" width="800"/>
  <br/><i>Dashboard</i><br/><br/>

  <img src="screenshots/departmen.png" alt="Departman" width="800"/>
  <br/><i>Departmanlar</i><br/><br/>

  <img src="screenshots/login.png" alt="Login Ekranı" width="800"/>
  <br/><i>Login Ekranı</i><br/><br/>

  <img src="screenshots/register.png" alt="Kayıt Ekranı" width="800"/>
  <br/><i>Kayıt Ekranı</i>
</div>
Diğer ekran görüntülerine screenshots adlı klasörden ulaşabilirsiniz.

## 🛠 Kurulum ve Çalıştırma

1. **Bağımlılıklar:** .NET 8.0 SDK ve SQL Server kurulumunun yapılmış olduğundan emin olun.
2. **Yapılandırma:** Her iki projenin (`ApiProject` ve `MvcProject`) `appsettings.json` dosyalarındaki `ConnectionStrings` kısmını kendi SQL Server yapılandırmanıza göre güncelleyin.
3. **Veritabanı değişiklikleri:**

   ```bash
   # ApiProject için
   cd ApiProject && dotnet ef database update

   # MvcProject için
   cd ../MvcProject && dotnet ef database update
   ```

4.
    ```bash
   # Çalıştırma:
   dotnet run
   ```

## 🚀 Adım Adım Nasıl Çalıştırılır?

Bu proje, arka uç (API) ve ön uç (MVC) olmak üzere iki ayrı uygulamadan oluşmaktadır. Doğru çalışması için sırasıyla ikisinin de çalıştırılması gerekir:

1. **Veritabanı Ayarları (API Projesi için):**
   - Eğer projede bir veritabanı bağlantısı varsa, `API-MVC/ApiProject/appsettings.json` içerisindeki bağlantı dizesini kontrol edin ve güncelleyin.
   - *(Gerekliyse `dotnet ef database update --project API-MVC/ApiProject/ApiProject.csproj` komutu ile veritabanını oluşturun.)*

2. **Önce API Projesini Başlatma:**
   - Ana depo kök dizininden (`SOFTITO-BACKEND`) bir terminal açın ve API'yi ayağa kaldırın:
   ```bash
   dotnet run --project API-MVC/ApiProject/ApiProject.csproj
   ```

3. **MVC (İstemci) Projesini Başlatma:**
   - **Yeni bir terminal sekmesi** açın (önceki terminal çalışmaya devam etmeli).
   - Ana dizinden MVC uygulamasını başlatın:
   ```bash
   dotnet run --project API-MVC/MvcProject/MvcProject.csproj
   ```

4. **Kullanım:**
   - MVC projesinin terminalde verdiği URL üzerinden tarayıcınızda uygulamaya giriş yapabilirsiniz. Uygulama, verileri arka planda çalışan API projesinden çekecektir.