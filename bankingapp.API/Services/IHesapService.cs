using bankingapp.API.DTOs.Hesap;

namespace bankingapp.API.Services
{
    public interface IHesapService
    {
        Task<List<HesapResponseDto>> GetAllAsync();
        Task<HesapResponseDto?> GetByIdAsync(int id);
        Task<List<HesapResponseDto>> GetByMusteriIdAsync(int musteriId);
        Task<HesapResponseDto> CreateAsync(HesapCreateDto dto);
        Task<bool> KapatAsync(int id);
    }
}