# HangfireConsoleApp (Arka Plan Görev Yöneticisi)

## Açýklama
HangfireConsoleApp, Hangfire ve SQL Server kullanarak arka planda periyodik olarak WebAPI’den kullanýcý ve ürün verisi çeken bir .NET 9 console/web uygulamasýdýr. Hangfire Dashboard ile iþleri izleyebilirsiniz.

## Özellikler
- Hangfire ile zamanlanmýþ iþler (kullanýcý ve ürün çekme)
- JWT ile API’ye kimlikli eriþim
- SQL Server ile Hangfire job storage
- Hangfire Dashboard arayüzü

## Kurulum
1. SQL Server’ýn çalýþtýðýndan emin olun ve baðlantý stringini güncelleyin.
2. Gerekli NuGet paketlerini yükleyin (`Hangfire`, `Hangfire.SqlServer`).
3. WebAPI projesinin çalýþýr durumda olduðundan emin olun.

## Çalýþtýrma
dotnet run veya Visual Studio üzerinden F5 ile baþlatabilirsiniz.

## Notlar
- Hangfire Dashboard’a [http://localhost:5000/hangfire](http://localhost:5000/hangfire) adresinden eriþebilirsiniz.
- API eriþimi için login bilgileri ve endpointler `Program.cs` içinde tanýmlýdýr.