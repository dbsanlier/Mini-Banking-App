using Microsoft.EntityFrameworkCore;
using bankingapp.API.Data;
using bankingapp.API.Entities;

namespace bankingapp.API.Repositories
{
    public class IslemRepository : IIslemRepository
    {
        private readonly AppDbContext _context;

        public IslemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Islem>> GetByHesapIdAsync(int hesapId)
        {
            return await _context.Islemler
                .Include(i => i.GonderenHesap)
                .Include(i => i.AliciHesap)
                .Where(i => i.GonderenHesapId == hesapId || i.AliciHesapId == hesapId)
                .OrderByDescending(i => i.IslemTarihi)
                .ToListAsync();
        }

        public async Task<Hesap?> GetHesapByIdAsync(int hesapId)
        {
            return await _context.Hesaplar.FirstOrDefaultAsync(h => h.Id == hesapId);
        }

        public async Task<bool> ParaYatirAsync(Hesap hesap, decimal tutar, string? aciklama, Islem islem)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                hesap.Bakiye += tutar;
                _context.Hesaplar.Update(hesap);
                await _context.Islemler.AddAsync(islem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ParaCekAsync(Hesap hesap, decimal tutar, string? aciklama, Islem islem)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                hesap.Bakiye -= tutar;
                _context.Hesaplar.Update(hesap);
                await _context.Islemler.AddAsync(islem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> TransferYapAsync(Hesap gonderenHesap, Hesap aliciHesap, decimal tutar, Islem islem)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                gonderenHesap.Bakiye -= tutar;
                aliciHesap.Bakiye += tutar;

                _context.Hesaplar.Update(gonderenHesap);
                _context.Hesaplar.Update(aliciHesap);
                await _context.Islemler.AddAsync(islem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}