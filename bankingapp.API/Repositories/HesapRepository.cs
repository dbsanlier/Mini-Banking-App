using Microsoft.EntityFrameworkCore;
using bankingapp.API.Data;
using bankingapp.API.Entities;

// veritabani ile konusan katman
// once interface olusturuyoruz, sonra onu implement ediyoruz
// interface sadece bu is icin hangi metotlar olmali diyor
// implementasyon ise bu metotlarin gercek kodlarini yaziyor
// service katmani interface ile konusuyor, repository katmaninin implementasyonunu bilmiyor
// bu sayede implementasyon degistiginde service katmani etkilenmiyor
// buna "Dependency Inversion Principle" deniyor
// yani ust seviyedeki katman alt seviyedeki katmana bagimli olmamali, ikisi de interface'e bagimli olmali
namespace bankingapp.API.Repositories
{
    public class HesapRepository : IHesapRepository
    {
        private readonly AppDbContext _context;

        public HesapRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hesap>> GetAllAsync()
        {
            return await _context.Hesaplar
                .Include(h => h.Musteri)
                .ToListAsync();
        }

        public async Task<Hesap?> GetByIdAsync(int id)
        {
            return await _context.Hesaplar
                .Include(h => h.Musteri)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<List<Hesap>> GetByMusteriIdAsync(int musteriId)
        {
            return await _context.Hesaplar
                .Include(h => h.Musteri)
                .Where(h => h.MusteriId == musteriId)
                .ToListAsync();
        }

        public async Task<bool> HesapNumarasiExistsAsync(string hesapNumarasi)
        {
            return await _context.Hesaplar.AnyAsync(h => h.HesapNumarasi == hesapNumarasi);
        }

        public async Task<bool> IbanExistsAsync(string iban)
        {
            return await _context.Hesaplar.AnyAsync(h => h.Iban == iban);
        }

        public async Task AddAsync(Hesap hesap)
        {
            await _context.Hesaplar.AddAsync(hesap);
        }

        public void Update(Hesap hesap)
        {
            _context.Hesaplar.Update(hesap);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}