using bankingapp.API.DTOs.Musteri;
using bankingapp.API.Entities;
using bankingapp.API.Repositories;

namespace bankingapp.API.Services
{
    public class MusteriService : IMusteriService
    {
        private readonly IMusteriRepository _repository;

        public MusteriService(IMusteriRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MusteriResponseDto>> GetAllAsync()
        {
            var musteriler = await _repository.GetAllAsync();
            return musteriler.Select(MapToResponseDto).ToList();
        }

        public async Task<MusteriResponseDto?> GetByIdAsync(int id)
        {
            var musteri = await _repository.GetByIdAsync(id);
            return musteri is null ? null : MapToResponseDto(musteri);
        }

        public async Task<List<MusteriResponseDto>> SearchAsync(string searchTerm)
        {
            var musteriler = await _repository.SearchAsync(searchTerm);
            return musteriler.Select(MapToResponseDto).ToList();
        }

        public async Task<MusteriResponseDto> CreateAsync(MusteriCreateDto dto)
        {
            // Is kurali: ayni TC Kimlik No ile kayitli musteri var mi kontrol et
            var mevcutMusteri = await _repository.GetByTcKimlikNoAsync(dto.TcKimlikNo);
            if (mevcutMusteri is not null)
            {
                throw new InvalidOperationException("Bu TC Kimlik Numarasi ile kayitli bir musteri zaten mevcut.");
            }

            var musteri = new Musteri
            {
                Ad = dto.Ad,
                Soyad = dto.Soyad,
                TcKimlikNo = dto.TcKimlikNo,
                Telefon = dto.Telefon,
                Email = dto.Email,
                OlusturulmaTarihi = DateTime.UtcNow
            };

            await _repository.AddAsync(musteri);
            await _repository.SaveChangesAsync();

            return MapToResponseDto(musteri);
        }

        public async Task<bool> UpdateAsync(int id, MusteriUpdateDto dto)
        {
            var musteri = await _repository.GetByIdAsync(id);
            if (musteri is null)
            {
                return false;
            }

            musteri.Ad = dto.Ad;
            musteri.Soyad = dto.Soyad;
            musteri.Telefon = dto.Telefon;
            musteri.Email = dto.Email;

            _repository.Update(musteri);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var musteri = await _repository.GetByIdAsync(id);
            if (musteri is null)
            {
                return false;
            }

            _repository.Delete(musteri);
            return await _repository.SaveChangesAsync();
        }

        // Entity -> DTO donusum metodu (tekrar kullanmamak icin ortak bir yerde)
        private static MusteriResponseDto MapToResponseDto(Musteri musteri)
        {
            return new MusteriResponseDto
            {
                Id = musteri.Id,
                Ad = musteri.Ad,
                Soyad = musteri.Soyad,
                TcKimlikNo = musteri.TcKimlikNo,
                Telefon = musteri.Telefon,
                Email = musteri.Email,
                OlusturulmaTarihi = musteri.OlusturulmaTarihi
            };
        }
    }
}