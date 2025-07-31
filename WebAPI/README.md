# WebAPI (.NET 9 Web API)

## A��klama
WebAPI, �r�n, kullan�c� ve log y�netimi sa�layan, JWT tabanl� kimlik do�rulama ve rate limiting i�eren bir .NET 9 Web API projesidir. Redis ile cache, SQL Server ile veri saklama ve Swagger/OpenAPI deste�i sunar.

## �zellikler
- JWT ile kimlik do�rulama
- �r�n, kullan�c� ve log CRUD i�lemleri
- Redis ile cache
- Rate Limiting
- Swagger/OpenAPI dok�mantasyonu
- Exception handling ve merkezi hata y�netimi

## Kurulum
1. Gerekli NuGet paketlerini y�kleyin (`Microsoft.EntityFrameworkCore.SqlServer`, `StackExchange.Redis`, `Swashbuckle.AspNetCore` vb.).
2. `appsettings.json` dosyas�ndaki ba�lant� stringlerini ve JWT ayarlar�n� d�zenleyin.
3. SQL Server ve Redis�in �al��t���ndan emin olun.

## �al��t�rma
dotnet run veya Visual Studio �zerinden ba�latabilirsiniz.

## Notlar
- API, HTTPS �zerinden �al��acak �ekilde yap�land�r�lm��t�r.
- Swagger aray�z�ne `/swagger` adresinden eri�ebilirsiniz.