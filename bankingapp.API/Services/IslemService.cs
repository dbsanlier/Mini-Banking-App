using bankingapp.API.DTOs.Islem;
using bankingapp.API.Entities;
using bankingapp.API.Repositories;

namespace bankingapp.API.Services
{
    public class IslemService : IIslemService
    {
        private readonly IIslemRepository _islemRepository;

        public IslemService(IIslemRepository islemRepository)
        {
            _islemRepository = islemRepository;
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

            if (dto.GonderenHesapId == dto.AliciHesapId)
            {
                throw new InvalidOperationException("Ayni hesaba transfer yapilamaz.");
            }

            var gonderenHesap = await _islemRepository.GetHesapByIdAsync(dto.GonderenHesapId);
            if (gonderenHesap is null)
            {
                throw new InvalidOperationException($"ID {dto.GonderenHesapId} ile gonderen hesap bulunamadi.");
            }

            var aliciHesap = await _islemRepository.GetHesapByIdAsync(dto.AliciHesapId);
            if (aliciHesap is null)
            {
                throw new InvalidOperationException($"ID {dto.AliciHesapId} ile alici hesap bulunamadi.");
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
                AliciHesapId = dto.AliciHesapId
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