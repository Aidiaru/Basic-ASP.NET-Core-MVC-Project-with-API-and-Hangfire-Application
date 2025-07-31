# HangfireConsoleApp (Arka Plan G�rev Y�neticisi)

## A��klama
HangfireConsoleApp, Hangfire ve SQL Server kullanarak arka planda periyodik olarak WebAPI�den kullan�c� ve �r�n verisi �eken bir .NET 9 console/web uygulamas�d�r. Hangfire Dashboard ile i�leri izleyebilirsiniz.

## �zellikler
- Hangfire ile zamanlanm�� i�ler (kullan�c� ve �r�n �ekme)
- JWT ile API�ye kimlikli eri�im
- SQL Server ile Hangfire job storage
- Hangfire Dashboard aray�z�

## Kurulum
1. SQL Server��n �al��t���ndan emin olun ve ba�lant� stringini g�ncelleyin.
2. Gerekli NuGet paketlerini y�kleyin (`Hangfire`, `Hangfire.SqlServer`).
3. WebAPI projesinin �al���r durumda oldu�undan emin olun.

## �al��t�rma
dotnet run veya Visual Studio �zerinden F5 ile ba�latabilirsiniz.

## Notlar
- Hangfire Dashboard�a [http://localhost:5000/hangfire](http://localhost:5000/hangfire) adresinden eri�ebilirsiniz.
- API eri�imi i�in login bilgileri ve endpointler `Program.cs` i�inde tan�ml�d�r.