using Microsoft.EntityFrameworkCore;
using bankingapp.API.Data;
using bankingapp.API.Entities;

namespace bankingapp.API.Repositories
{
    public class MusteriRepository : IMusteriRepository
    {
        private readonly AppDbContext _context;

        public MusteriRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Musteri>> GetAllAsync()
        {
            return await _context.Musteriler.ToListAsync();
        }

        public async Task<Musteri?> GetByIdAsync(int id)
        {
            return await _context.Musteriler.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Musteri?> GetByTcKimlikNoAsync(string tcKimlikNo)
        {
            return await _context.Musteriler.FirstOrDefaultAsync(m => m.TcKimlikNo == tcKimlikNo);
        }

        public async Task<List<Musteri>> SearchAsync(string searchTerm)
        {
            return await _context.Musteriler
                .Where(m => m.Ad.Contains(searchTerm) ||
                            m.Soyad.Contains(searchTerm) ||
                            m.TcKimlikNo.Contains(searchTerm) ||
                            m.Email.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task AddAsync(Musteri musteri)
        {
            await _context.Musteriler.AddAsync(musteri);
        }

        public void Update(Musteri musteri)
        {
            _context.Musteriler.Update(musteri);
        }

        public void Delete(Musteri musteri)
        {
            _context.Musteriler.Remove(musteri);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}