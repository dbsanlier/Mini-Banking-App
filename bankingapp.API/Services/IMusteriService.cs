using bankingapp.API.DTOs.Musteri;

namespace bankingapp.API.Services
{
    public interface IMusteriService
    {
        Task<List<MusteriResponseDto>> GetAllAsync();
        Task<MusteriResponseDto?> GetByIdAsync(int id);
        Task<List<MusteriResponseDto>> SearchAsync(string searchTerm);
        Task<MusteriResponseDto> CreateAsync(MusteriCreateDto dto);
        Task<bool> UpdateAsync(int id, MusteriUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}