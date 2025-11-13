# 🚀 TwitSharp API
Twitter benzeri sosyal medya servisinin **ASP.NET Core** ile geliştirilmiş backend projesi.

✔ Kullanıcı yönetimi  
✔ Kimlik doğrulama  
✔ Gönderi paylaşımı  
✔ Beğeni – Yorum sistemi  
✔ **JWT + Refresh Token** desteği  
✔ Clean Architecture  

içeren, çok katmanlı profesyonel bir backend mimarisi sunar.

## 📌 Özellikler (Features)
|Özellik | Açıklama |
|--------|------------|
👤 Kullanıcı Yönetimi |	Register, Login, Profil
📝 Post Paylaşımı     |	Gönderi oluşturma & listeleme
❤️ Beğeni Sistemi     |	Like / Unlike işlemleri
💬 Yorum Sistemi      |	Post’a yorum ekleme
🔑 JWT Auth           |	Login – Refresh Token mekanizması
🧱 Clean Architecture |	Katmanlı profesyonel backend yapısı


## 🏛 Mimari Yapı

Proje **Clean Architecture / Onion Architecture** prensipleriyle katmanlı olarak tasarlanmıştır:


### 🔹 1. Entity Katmanı
📌 Veritabanı modelleri  
📌 DTO (Data Transfer Objects)  
📌 Domain nesneleri  


### 🔹 2. DataAccess Katmanı
📁 TwitSharpContext (DbContext)  
📁 Repository implementasyonları  


### 🔹 3. Business Katmanı
⚙ Servis arayüzleri (IUserService, IPostService, vb.)  
⚙ Manager sınıfları  
⚙ İş kuralları ve validasyon  
⚙ Token üretimi & Refresh Token işlemleri  
⚙ Dependency Injection (DI) kullanımı  


### 🔹 4. Core Katmanı
📌 Ortak altyapı:
- Result yapıları (SuccessResult, ErrorResult…)  
- JWT Helper  
- Security & Hashing  
- Generic Repository Interface  
- Ortak interface’ler (IEntity, IDto)


### 🔹 5. WebAPI Katmanı
🌐 Controller’lar  
🔐 JWT Auth Middleware  
⚙ DTO – Model dönüşümleri  
🛠 appsettings.json yapılandırmaları  


## 🔐 Kimlik Doğrulama Sistemi

Projede modern bir **JWT Authentication sistemi** uygulanmıştır.

✔ Access Token  
✔ Refresh Token  
✔ Token Yenileme  
✔ Refresh Token’ın DB’de saklanması  

Kısaca:  
➡ Login/Register → **Access + Refresh Token** oluşturulur  
➡ Access Token süresi dolduğunda → Refresh Token ile yenilenir  


## 🧪 Kurulum Adımları

### 1️⃣ Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/ssms/install/install)

### 2️⃣ Repoyu klonla
   ```bash
   git clone https://github.com/tolgadirek/twitSharp_backend.git
   ```

### 3️⃣ Veritabanı Bağlantısını Yapılandır
  `WebAPI/appsettings.json` içindeki ConnectionStrings bölümünü kendi SQL bağlantına göre düzenle.

### 4️⃣ Migration Oluştur ve Veritabanını Güncelle
```bash
  dotnet ef migrations add MigrationName -p DataAccess -s WebAPI
  dotnet ef database update -p DataAccess -s WebAPI
```

### 5️⃣ JWT Secret Key Ekle
  `appsettings.json → Jwt → Key` içine kendi secret key’ini yaz.

### 5️⃣ Uygulamayı çalıştır
```bash
cd WebAPI
dotnet run
```

