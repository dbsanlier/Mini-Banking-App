using bankingapp.API.Entities;

namespace bankingapp.API.Repositories
{
    public interface IMusteriRepository
    {
        Task<List<Musteri>> GetAllAsync();
        Task<Musteri?> GetByIdAsync(int id);
        Task<Musteri?> GetByTcKimlikNoAsync(string tcKimlikNo);
        Task<List<Musteri>> SearchAsync(string searchTerm);
        Task AddAsync(Musteri musteri);
        void Update(Musteri musteri);
        void Delete(Musteri musteri);
        Task<bool> SaveChangesAsync();
    }
}