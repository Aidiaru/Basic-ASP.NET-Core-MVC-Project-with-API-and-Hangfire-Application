# WebApplication1 (Razor Pages/MVC Web Uygulamasý)

## Açýklama
WebApplication1, kullanýcý yönetimi, ürün listeleme ve log takibi gibi iþlevler sunan, JWT tabanlý kimlik doðrulama kullanan bir ASP.NET Core Razor Pages/MVC uygulamasýdýr. Arka planda bir WebAPI ile haberleþir.

## Özellikler
- JWT ile oturum yönetimi
- Ürün listeleme, ekleme, düzenleme ve silme
- Kullanýcý yönetimi (Admin paneli)
- Log görüntüleme (Admin)
- Bootstrap ve jQuery ile modern arayüz

## Kurulum
1. Gerekli NuGet paketlerini yükleyin.
2. `appsettings.json` dosyasýndaki API adreslerini ve baðlantý ayarlarýný kontrol edin.
3. Projeyi Visual Studio 2022 ile açýn ve çalýþtýrýn.

## Çalýþtýrma
dotnet run veya Visual Studio üzerinden F5 ile baþlatabilirsiniz.

## Notlar
- API ile haberleþmek için arka planda çalýþan WebAPI projesinin de çalýþýyor olmasý gerekir.
- Oturum için JWT token’ý session’da tutulur.

