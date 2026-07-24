using bankingapp.API.DTOs.Hesap;
using bankingapp.API.Entities;
using bankingapp.API.Repositories;
// is kurallarinin yasadigi yer
// repo 'getir, ekle, sil, guncelle' gibi islemleri gercekliyor, ama is kurallarini bilmez
// service ise 'bu islem mantiken dogru mu, bu islem icin gerekli kosullar saglaniyor mu' gibi kontrolleri yapar
// service katmani repository katmanina bagimli, ama repository katmani service katmanina bagimli degil
namespace bankingapp.API.Services
{
    public class HesapService : IHesapService
    {
        private readonly IHesapRepository _hesapRepository;
        private readonly IMusteriRepository _musteriRepository;

        public HesapService(IHesapRepository hesapRepository, IMusteriRepository musteriRepository)
        {
            _hesapRepository = hesapRepository;
            _musteriRepository = musteriRepository;
        }

        public async Task<List<HesapResponseDto>> GetAllAsync()
        {
            var hesaplar = await _hesapRepository.GetAllAsync();
            return hesaplar.Select(MapToResponseDto).ToList();
        }

        public async Task<HesapResponseDto?> GetByIdAsync(int id)
        {
            var hesap = await _hesapRepository.GetByIdAsync(id);
            return hesap is null ? null : MapToResponseDto(hesap);
        }

        public async Task<List<HesapResponseDto>> GetByMusteriIdAsync(int musteriId)
        {
            var hesaplar = await _hesapRepository.GetByMusteriIdAsync(musteriId);
            return hesaplar.Select(MapToResponseDto).ToList();
        }

        public async Task<HesapResponseDto> CreateAsync(HesapCreateDto dto)
        {
            // Musteri var mi kontrol et
            var musteri = await _musteriRepository.GetByIdAsync(dto.MusteriId);
            if (musteri is null)
            {
                throw new InvalidOperationException($"ID {dto.MusteriId} ile musteri bulunamadi.");
            }

            if (dto.BaslangicBakiyesi < 0)
            {
                throw new InvalidOperationException("Baslangic bakiyesi negatif olamaz.");
            }

            // Benzersiz Hesap Numarasi ve IBAN uret
            string hesapNumarasi;
            do
            {
                hesapNumarasi = GenerateHesapNumarasi();
            } while (await _hesapRepository.HesapNumarasiExistsAsync(hesapNumarasi));

            string iban;
            do
            {
                iban = GenerateIban(hesapNumarasi);
            } while (await _hesapRepository.IbanExistsAsync(iban));

            var hesap = new Hesap
            {
                MusteriId = dto.MusteriId,
                HesapNumarasi = hesapNumarasi,
                Iban = iban,
                Bakiye = dto.BaslangicBakiyesi,
                Durum = HesapDurumu.Aktif,
                OlusturulmaTarihi = DateTime.UtcNow
            };

            await _hesapRepository.AddAsync(hesap);
            await _hesapRepository.SaveChangesAsync();

            // Musteri bilgisini DTO'ya yansitmak icin tekrar cek (Include ile)
            var olusturulanHesap = await _hesapRepository.GetByIdAsync(hesap.Id);
            return MapToResponseDto(olusturulanHesap!);
        }

        public async Task<bool> KapatAsync(int id)
        {
            var hesap = await _hesapRepository.GetByIdAsync(id);
            if (hesap is null)
            {
                return false;
            }

            if (hesap.Bakiye != 0)
            {
                throw new InvalidOperationException("Bakiyesi sifir olmayan hesap kapatilamaz.");
            }

            hesap.Durum = HesapDurumu.Kapali;
            _hesapRepository.Update(hesap);
            return await _hesapRepository.SaveChangesAsync();
        }

        private static string GenerateHesapNumarasi()
        {
            // 10 haneli rastgele hesap numarasi
            var random = new Random();
            return random.NextInt64(1000000000, 9999999999).ToString();
        }

        private static string GenerateIban(string hesapNumarasi)
        {
            // Basitlestirilmis TR IBAN formati 
            var random = new Random();
            var bankaKodu = "00061"; // ornek banka kodu
            var kontrolBasamagi = random.Next(10, 99);
            return $"TR{kontrolBasamagi}{bankaKodu}0000000{hesapNumarasi}";
        }

        private static HesapResponseDto MapToResponseDto(Hesap hesap)
        {
            return new HesapResponseDto
            {
                Id = hesap.Id,
                HesapNumarasi = hesap.HesapNumarasi,
                Iban = hesap.Iban,
                Bakiye = hesap.Bakiye,
                Durum = hesap.Durum.ToString(),
                OlusturulmaTarihi = hesap.OlusturulmaTarihi,
                MusteriId = hesap.MusteriId,
                MusteriAdSoyad = hesap.Musteri is not null
                    ? $"{hesap.Musteri.Ad} {hesap.Musteri.Soyad}"
                    : string.Empty
            };
        }
    }
}