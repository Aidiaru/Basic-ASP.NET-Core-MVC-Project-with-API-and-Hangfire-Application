# WebApplication1 (Razor Pages/MVC Web Uygulamas�)

## A��klama
WebApplication1, kullan�c� y�netimi, �r�n listeleme ve log takibi gibi i�levler sunan, JWT tabanl� kimlik do�rulama kullanan bir ASP.NET Core Razor Pages/MVC uygulamas�d�r. Arka planda bir WebAPI ile haberle�ir.

## �zellikler
- JWT ile oturum y�netimi
- �r�n listeleme, ekleme, d�zenleme ve silme
- Kullan�c� y�netimi (Admin paneli)
- Log g�r�nt�leme (Admin)
- Bootstrap ve jQuery ile modern aray�z

## Kurulum
1. Gerekli NuGet paketlerini y�kleyin.
2. `appsettings.json` dosyas�ndaki API adreslerini ve ba�lant� ayarlar�n� kontrol edin.
3. Projeyi Visual Studio 2022 ile a��n ve �al��t�r�n.

## �al��t�rma
dotnet run veya Visual Studio �zerinden F5 ile ba�latabilirsiniz.

## Notlar
- API ile haberle�mek i�in arka planda �al��an WebAPI projesinin de �al���yor olmas� gerekir.
- Oturum i�in JWT token�� session�da tutulur.

