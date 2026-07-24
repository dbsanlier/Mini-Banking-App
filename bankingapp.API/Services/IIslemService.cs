using bankingapp.API.DTOs.Islem;

namespace bankingapp.API.Services
{
    public interface IIslemService
    {
        Task<List<IslemResponseDto>> GetByHesapIdAsync(int hesapId);
        Task<IslemResponseDto> ParaYatirAsync(ParaYatirmaDto dto);
        Task<IslemResponseDto> ParaCekAsync(ParaCekmeDto dto);
        Task<IslemResponseDto> TransferYapAsync(TransferDto dto);
    }
}