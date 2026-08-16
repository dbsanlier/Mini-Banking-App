# Mini Bankacılık Uygulaması

## Projenin Amacı

Bu proje, staj kapsamında geliştirilen küçük ölçekli bir bankacılık uygulamasıdır. React, .NET 10 Web API ve PostgreSQL kullanılarak geliştirilmiştir.

Projenin temel amacı; modern web uygulamalarının nasıl geliştirildiğini öğrenmek, katmanlı mimariyi anlamak ve yazılım geliştirme süreçlerine hakim olmaktır.

Uygulama; müşteri yönetimi, hesap yönetimi, para yatırma/çekme, para transferi, işlem geçmişi görüntüleme ve güncel döviz kurlarını takip etme özelliklerini içermektedir.

## Kullanılan Teknolojiler

### Backend
- .NET 10 Web API
- Entity Framework Core (Code First)
- Npgsql (Entity Framework Core PostgreSQL Provider)
- HttpClient (dış API entegrasyonu için)

### Frontend
- React (Vite ile oluşturuldu)
- React Router
- Axios

### Veritabanı
- PostgreSQL

### Versiyon Kontrol
- Git / GitHub

### Dış Servisler
- [open.er-api.com](https://www.exchangerate-api.com/) — güncel döviz kuru verisi (USD, EUR, GBP)

## Proje Mimarisi

Backend, katmanlı mimari kullanılarak geliştirilmiştir:

```
bankingapp.API/
├── Controllers/     # API endpoint'leri
├── Services/        # İş mantığı (business logic)
├── Repositories/    # Veritabanı erişim katmanı
├── Entities/         # Veritabanı tablolarını temsil eden sınıflar
├── DTOs/              # Veri transfer nesneleri
├── Middlewares/        # Merkezi hata yönetimi ve istek loglama
├── Data/               # DbContext
└── Migrations/         # EF Core migration dosyaları
```

Frontend, aşağıdaki yapı ile geliştirilmiştir:

```
bankingapp-ui/
├── src/
│   ├── pages/        # Sayfa bileşenleri (Ana Sayfa, Müşteri, Hesap, Yatırım)
│   ├── components/   # Tekrar kullanılabilir bileşenler (Layout)
│   ├── services/     # API çağrıları (Axios)
│   └── styles/       # Ortak stil dosyaları
```

## Kurulum Adımları

### 1. Ön Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/download/)
- Git

### 2. PostgreSQL Kurulumu

1. PostgreSQL'i [resmi siteden](https://www.postgresql.org/download/) indirip kurun.
2. Kurulum sırasında belirlediğiniz `postgres` kullanıcı şifresini not alın.

### 3. Veritabanının Oluşturulması

Tablolar migration ile **otomatik** oluşturulur, manuel tablo oluşturmaya gerek yoktur.

1. pgAdmin veya psql üzerinden boş bir veritabanı oluşturun:
   ```sql
   CREATE DATABASE "BankingAppDatabase";
   ```

### 4. Backend'in Çalıştırılması

1. Projeyi klonlayın:
   ```
   git clone https://github.com/dbsanlier/Mini-Banking-App.git
   cd Mini-Banking-App/bankingapp.API
   ```

2. Bağlantı dizesini `user-secrets` ile ayarlayın:
   ```
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=BankingAppDatabase;Username=postgres;Password=SIFRENIZ"
   ```

3. Gerekli paketleri yükleyin:
   ```
   dotnet restore
   ```

4. Migration'ları veritabanına uygulayın:
   ```
   dotnet ef database update
   ```

5. *(Opsiyonel)* Örnek veri seti eklemek isterseniz, proje kök dizinindeki `seed_data.sql` dosyasını pgAdmin Query Tool'da çalıştırın veya:
   ```
   psql -U postgres -d BankingAppDatabase -f seed_data.sql
   ```

6. Uygulamayı çalıştırın:
   ```
   dotnet run
   ```

7. Uygulama şu adreste çalışır:
   ```
   http://localhost:5260
   ```
   Swagger arayüzü:
   ```
   http://localhost:5260/swagger
   ```

### 5. Frontend'in Çalıştırılması

1. Frontend klasörüne gidin:
   ```
   cd ../bankingapp-ui
   ```

2. Bağımlılıkları yükleyin:
   ```
   npm install
   ```

3. Geliştirme sunucusunu başlatın:
   ```
   npm run dev
   ```

4. Tarayıcıda açın:
   ```
   http://localhost:5173
   ```

> Not: Backend, `http://localhost:5173` adresinden gelen isteklere izin verecek şekilde CORS ayarlıdır. Frontend'i farklı bir portta çalıştırırsanız `Program.cs` içindeki CORS politikasını güncellemeniz gerekir.

## API Endpoint'leri

### Müşteri
| Metot | Endpoint | Açıklama |
|-------|----------|----------|
| GET | `/api/musteri` | Tüm müşterileri listeler |
| GET | `/api/musteri/{id}` | ID'ye göre müşteri getirir |
| GET | `/api/musteri/search?term=` | Müşteri arar |
| POST | `/api/musteri` | Yeni müşteri oluşturur |
| PUT | `/api/musteri/{id}` | Müşteri bilgilerini günceller |
| DELETE | `/api/musteri/{id}` | Müşteriyi siler |

### Hesap
| Metot | Endpoint | Açıklama |
|-------|----------|----------|
| GET | `/api/hesap` | Tüm hesapları listeler |
| GET | `/api/hesap/{id}` | ID'ye göre hesap getirir |
| GET | `/api/hesap/musteri/{musteriId}` | Müşteriye ait hesapları listeler |
| POST | `/api/hesap` | Yeni hesap açar |
| PUT | `/api/hesap/{id}/kapat` | Hesabı kapatır |

### İşlem
| Metot | Endpoint | Açıklama |
|-------|----------|----------|
| GET | `/api/islem/hesap/{hesapId}` | Hesaba ait işlem geçmişini getirir |
| POST | `/api/islem/para-yatir` | Para yatırma işlemi yapar |
| POST | `/api/islem/para-cek` | Para çekme işlemi yapar |
| POST | `/api/islem/transfer` | Hesaplar arası transfer yapar |

### Döviz
| Metot | Endpoint | Açıklama |
|-------|----------|----------|
| GET | `/api/doviz/kurlar` | USD, EUR, GBP için güncel TL karşılıklarını getirir |

## İş Kuralları

- Hesap bakiyesi eksiye düşemez.
- Aynı hesaba transfer yapılamaz.
- Kapalı hesaplara işlem yapılamaz.
- Bakiyesi sıfır olmayan hesap kapatılamaz.
- Aynı TC Kimlik Numarası ile birden fazla müşteri kaydedilemez.
- Her işlem (yatırma/çekme/transfer) işlem geçmişine kaydedilir ve veritabanı transaction'ı ile bütünlüğü korunur.

## Middleware Yapısı

- **RequestLoggingMiddleware**: Gelen her isteğin HTTP metodunu, yolunu, durum kodunu ve süresini loglar.
- **ExceptionHandlingMiddleware**: Uygulama genelinde yakalanmamış hataları merkezi olarak yakalar, kullanıcıya güvenli bir hata mesajı döner ve detayları sunucu loglarına yazar.

## Notlar

- Bu proje bir öğrenme çalışmasıdır; tüm özelliklerin eksiksiz olması hedeflenmemiştir.
- Veritabanı şifresi gibi hassas bilgiler `.NET User Secrets` ile saklanmakta olup kaynak kodunda yer almamaktadır.
- Döviz kuru verisi ücretsiz bir dış servisten (open.er-api.com) çekilmektedir; servis geçici olarak erişilemezse ilgili para birimi listede görünmeyebilir.
