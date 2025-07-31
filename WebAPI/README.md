# WebAPI (.NET 9 Web API)

## Açýklama
WebAPI, ürün, kullanýcý ve log yönetimi saðlayan, JWT tabanlý kimlik doðrulama ve rate limiting içeren bir .NET 9 Web API projesidir. Redis ile cache, SQL Server ile veri saklama ve Swagger/OpenAPI desteði sunar.

## Özellikler
- JWT ile kimlik doðrulama
- Ürün, kullanýcý ve log CRUD iþlemleri
- Redis ile cache
- Rate Limiting
- Swagger/OpenAPI dokümantasyonu
- Exception handling ve merkezi hata yönetimi

## Kurulum
1. Gerekli NuGet paketlerini yükleyin (`Microsoft.EntityFrameworkCore.SqlServer`, `StackExchange.Redis`, `Swashbuckle.AspNetCore` vb.).
2. `appsettings.json` dosyasýndaki baðlantý stringlerini ve JWT ayarlarýný düzenleyin.
3. SQL Server ve Redis’in çalýþtýðýndan emin olun.

## Çalýþtýrma
dotnet run veya Visual Studio üzerinden baþlatabilirsiniz.

## Notlar
- API, HTTPS üzerinden çalýþacak þekilde yapýlandýrýlmýþtýr.
- Swagger arayüzüne `/swagger` adresinden eriþebilirsiniz.