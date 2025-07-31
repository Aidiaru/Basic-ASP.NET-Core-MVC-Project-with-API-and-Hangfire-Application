# .NET 9 Çok Katmanlı Uygulama Çözümü

Bu repository, modern bir .NET 9 ekosisteminde geliştirilmiş, çok katmanlı bir uygulama örneği içerir. Çözümde; bir WebAPI, bir Razor Pages/MVC tabanlı web uygulaması ve Hangfire ile arka plan görevlerini yöneten bir servis bulunmaktadır.

## Proje Yapısı

- **WebAPI**  
  .NET 9 ile geliştirilmiş, JWT tabanlı kimlik doğrulama, ürün/kullanıcı/log yönetimi, Redis cache, rate limiting ve Swagger/OpenAPI desteği sunan RESTful API projesi.

- **WebApplication1**  
  Razor Pages/MVC mimarisiyle geliştirilmiş, kullanıcıların ürünleri ve kullanıcıları yönetebildiği, JWT ile oturum açma ve rol tabanlı erişim sağlayan modern bir web arayüzü.

- **HangfireConsoleApp**  
  Hangfire ve SQL Server kullanarak arka planda periyodik olarak WebAPI’den veri çeken ve işleri izlemek için dashboard sunan bir arka plan servis uygulaması.

## Başlıca Özellikler

- JWT ile kimlik doğrulama ve yetkilendirme
- Ürün, kullanıcı ve log yönetimi (CRUD)
- Redis ile cache mekanizması
- Rate limiting (istek kısıtlama)
- Swagger/OpenAPI ile API dokümantasyonu
- Hangfire ile zamanlanmış arka plan görevleri
- Modern ve kullanıcı dostu arayüz (Bootstrap, jQuery)
- SQL Server ile veri saklama

## Kurulum ve Çalıştırma

### 1. Ortam Gereksinimleri
- .NET 9 SDK
- SQL Server (ör. Express veya Docker ile)
- Redis (ör. Docker ile)
- Visual Studio 2022 veya VS Code

### 2. Bağımlılıkların Yüklenmesi
Her proje klasöründe aşağıdaki komutu çalıştırarak NuGet bağımlılıklarını yükleyin:
dotnet restore


### 3. Veritabanı ve Redis
- SQL Server ve Redis servislerinin çalıştığından emin olun.
- Gerekirse `appsettings.json` dosyalarındaki bağlantı ayarlarını güncelleyin.

### 4. Projeleri Çalıştırma

#### WebAPI
cd WebAPI dotnet run
Swagger arayüzüne [https://localhost:7209/swagger](https://localhost:7209/swagger) adresinden erişebilirsiniz.

#### WebApplication1
cd WebApplication1 dotnet run
Uygulamaya [https://localhost:7299](https://localhost:7299) adresinden erişebilirsiniz.

#### HangfireConsoleApp
cd HangfireConsoleApp dotnet run
Hangfire Dashboard: [http://localhost:5000/hangfire](http://localhost:5000/hangfire)

## Notlar

- Tüm projelerin aynı anda çalışıyor olması gerekir.
- JWT ile kimlik doğrulama için WebAPI üzerinden login olmanız gerekmektedir.
- Geliştirme ortamında HTTPS sertifikası gereklidir.
- Varsayılan kullanıcı ve şifreler ile ilgili bilgileri ilgili API veya dokümantasyondan kontrol ediniz.

## Katkı ve Lisans

Bu proje eğitim ve örnek amaçlıdır. Katkıda bulunmak için fork’layabilir ve pull request gönderebilirsiniz.

---

