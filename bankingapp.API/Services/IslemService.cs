using bankingapp.API.DTOs.Islem;
using bankingapp.API.Entities;
using bankingapp.API.Repositories;

namespace bankingapp.API.Services
{
    public class IslemService : IIslemService
    {
        private readonly IIslemRepository _islemRepository;
        private readonly IHesapRepository _hesapRepository;

        public IslemService(IIslemRepository islemRepository, IHesapRepository hesapRepository)
        {
            _islemRepository = islemRepository;
            _hesapRepository = hesapRepository;
        }

        public async Task<List<IslemResponseDto>> GetByHesapIdAsync(int hesapId)
        {
    var islemler = await _islemRepository.GetByHesapIdAsync(hesapId);
    return islemler.Select(i => MapToResponseDto(
        i,
        i.GonderenHesap?.HesapNumarasi,
        i.AliciHesap?.HesapNumarasi
    )).ToList();
        }

        public async Task<IslemResponseDto> ParaYatirAsync(ParaYatirmaDto dto)
        {
            if (dto.Tutar <= 0)
            {
                throw new InvalidOperationException("Yatirilacak tutar sifirdan buyuk olmalidir.");
            }

            var hesap = await _islemRepository.GetHesapByIdAsync(dto.HesapId);
            if (hesap is null)
            {
                throw new InvalidOperationException($"ID {dto.HesapId} ile hesap bulunamadi.");
            }

            if (hesap.Durum == HesapDurumu.Kapali)
            {
                throw new InvalidOperationException("Kapali hesaba islem yapilamaz.");
            }

            var islem = new Islem
            {
                Tipi = IslemTipi.ParaYatirma,
                Tutar = dto.Tutar,
                Aciklama = dto.Aciklama,
                IslemTarihi = DateTime.UtcNow,
                GonderenHesapId = dto.HesapId
            };

            await _islemRepository.ParaYatirAsync(hesap, dto.Tutar, dto.Aciklama, islem);

            return MapToResponseDto(islem, hesap.HesapNumarasi, null);
        }

        public async Task<IslemResponseDto> ParaCekAsync(ParaCekmeDto dto)
        {
            if (dto.Tutar <= 0)
            {
                throw new InvalidOperationException("Cekilecek tutar sifirdan buyuk olmalidir.");
            }

            var hesap = await _islemRepository.GetHesapByIdAsync(dto.HesapId);
            if (hesap is null)
            {
                throw new InvalidOperationException($"ID {dto.HesapId} ile hesap bulunamadi.");
            }

            if (hesap.Durum == HesapDurumu.Kapali)
            {
                throw new InvalidOperationException("Kapali hesaba islem yapilamaz.");
            }

            if (hesap.Bakiye < dto.Tutar)
            {
                throw new InvalidOperationException("Yetersiz bakiye.");
            }

            var islem = new Islem
            {
                Tipi = IslemTipi.ParaCekme,
                Tutar = dto.Tutar,
                Aciklama = dto.Aciklama,
                IslemTarihi = DateTime.UtcNow,
                GonderenHesapId = dto.HesapId
            };

            await _islemRepository.ParaCekAsync(hesap, dto.Tutar, dto.Aciklama, islem);

            return MapToResponseDto(islem, hesap.HesapNumarasi, null);
        }

        public async Task<IslemResponseDto> TransferYapAsync(TransferDto dto)
        {
            if (dto.Tutar <= 0)
            {
                throw new InvalidOperationException("Transfer tutari sifirdan buyuk olmalidir.");
            }

            var gonderenHesap = await _islemRepository.GetHesapByIdAsync(dto.GonderenHesapId);
            if (gonderenHesap is null)
            {
                throw new InvalidOperationException($"ID {dto.GonderenHesapId} ile gonderen hesap bulunamadi.");
            }

            // Alici, hesap ID ile degil IBAN ile bulunuyor; gonderen kisinin yazdigi
            // alici adi soyadi, IBAN'in gercek sahibiyle eslesmezse islem gerceklesmez.
            var aliciHesap = await _hesapRepository.GetByIbanAsync(dto.AliciIban.Trim());
            if (aliciHesap is null)
            {
                throw new InvalidOperationException("Bu IBAN'a ait hesap bulunamadi.");
            }

            var aliciGercekAdSoyad = $"{aliciHesap.Musteri?.Ad} {aliciHesap.Musteri?.Soyad}".Trim();
            if (!string.Equals(aliciGercekAdSoyad, dto.AliciAdSoyad.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Alici adi soyadi, IBAN sahibiyle eslesmiyor.");
            }

            if (gonderenHesap.Id == aliciHesap.Id)
            {
                throw new InvalidOperationException("Ayni hesaba transfer yapilamaz.");
            }

            if (gonderenHesap.Durum == HesapDurumu.Kapali || aliciHesap.Durum == HesapDurumu.Kapali)
            {
                throw new InvalidOperationException("Kapali hesaba/hesaptan islem yapilamaz.");
            }

            if (gonderenHesap.Bakiye < dto.Tutar)
            {
                throw new InvalidOperationException("Yetersiz bakiye.");
            }

            var islem = new Islem
            {
                Tipi = IslemTipi.Transfer,
                Tutar = dto.Tutar,
                Aciklama = dto.Aciklama,
                IslemTarihi = DateTime.UtcNow,
                GonderenHesapId = dto.GonderenHesapId,
                AliciHesapId = aliciHesap.Id
            };

            await _islemRepository.TransferYapAsync(gonderenHesap, aliciHesap, dto.Tutar, islem);

            return MapToResponseDto(islem, gonderenHesap.HesapNumarasi, aliciHesap.HesapNumarasi);
        }

        private static IslemResponseDto MapToResponseDto(Islem islem, string? gonderenHesapNo, string? aliciHesapNo)
        {
            return new IslemResponseDto
            {
                Id = islem.Id,
                Tipi = islem.Tipi.ToString(),
                Tutar = islem.Tutar,
                Aciklama = islem.Aciklama,
                IslemTarihi = islem.IslemTarihi,
                GonderenHesapNumarasi = gonderenHesapNo,
                AliciHesapNumarasi = aliciHesapNo
            };
        }
    }
}