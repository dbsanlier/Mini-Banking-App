using bankingapp.API.Entities;

namespace bankingapp.API.Repositories
{
    public interface IIslemRepository
    {
        Task<List<Islem>> GetByHesapIdAsync(int hesapId);
        Task<Hesap?> GetHesapByIdAsync(int hesapId);
        Task<bool> ParaYatirAsync(Hesap hesap, decimal tutar, string? aciklama, Islem islem);
        Task<bool> ParaCekAsync(Hesap hesap, decimal tutar, string? aciklama, Islem islem);
        Task<bool> TransferYapAsync(Hesap gonderenHesap, Hesap aliciHesap, decimal tutar, Islem islem);
    }
}