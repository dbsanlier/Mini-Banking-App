using bankingapp.API.Entities;

namespace bankingapp.API.Repositories
{
    public interface IHesapRepository
    {
        Task<List<Hesap>> GetAllAsync();
        Task<Hesap?> GetByIdAsync(int id);
        Task<Hesap?> GetByIbanAsync(string iban);
        Task<List<Hesap>> GetByMusteriIdAsync(int musteriId);
        Task<bool> HesapNumarasiExistsAsync(string hesapNumarasi);
        Task<bool> IbanExistsAsync(string iban);
        Task AddAsync(Hesap hesap);
        void Update(Hesap hesap);
        Task<bool> SaveChangesAsync();
    }
}