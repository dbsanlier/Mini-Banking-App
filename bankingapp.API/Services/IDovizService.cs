using bankingapp.API.DTOs.Doviz;

namespace bankingapp.API.Services
{
    public interface IDovizService
    {
        Task<List<DovizKuruDto>> GetKurlarAsync();
    }
}