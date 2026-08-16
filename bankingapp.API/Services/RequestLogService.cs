using bankingapp.API.Data;
using bankingapp.API.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace bankingapp.API.Services
{
    // Bu servis diger servislerden farkli olarak Repository katmani kullanmiyor:
    // tek yaptigi is bir log satiri eklemek, is kurali icermiyor.
    //
    // Istegin kendi AppDbContext'ini (constructor injection) degil, ayri bir
    // scope/DbContext kullaniyoruz. Cunku istek sirasinda bir domain islemi
    // (ör. Musteri silme) SaveChangesAsync'te hata alirsa, ayni context'te
    // hala "kirli" (basarisiz) degisiklikler takip ediliyor olabilir; log
    // kaydini o context ile yazmaya calismak ayni hatayi tekrar tetikleyip
    // log yazimini da basarisiz kilar.
    public class RequestLogService : IRequestLogService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RequestLogService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task LogAsync(RequestLog log)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.RequestLogs.Add(log);
            await context.SaveChangesAsync();
        }
    }
}
