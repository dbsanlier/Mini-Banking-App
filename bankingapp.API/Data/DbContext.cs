using Microsoft.EntityFrameworkCore;
using bankingapp.API.Entities;

// DbContext, Entity Framework Core'un temel sinifi
// veritabanı ile etkilesim kurmak için kullanilir
// AppDbContext sinifi da uygulamanin veritabanı baglamini temsil eder ve 
// DbSet<T> özellikleri ile veritabanındaki tablolarla etkilesim saglar

//Repository katmani bu kopruyu kullanarak gercek SQL komutlarini arka planda  otomatik uretiyo
namespace bankingapp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Hesap> Hesaplar { get; set; }
        public DbSet<Islem> Islemler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Musteri - Hesap iliskisi (bire-cok)
            modelBuilder.Entity<Hesap>()
                .HasOne(h => h.Musteri)
                .WithMany(m => m.Hesaplar)
                .HasForeignKey(h => h.MusteriId)
                .OnDelete(DeleteBehavior.Cascade);

            // Islem - GonderenHesap iliskisi
            modelBuilder.Entity<Islem>()
                .HasOne(i => i.GonderenHesap)
                .WithMany(h => h.GonderilenIslemler)
                .HasForeignKey(i => i.GonderenHesapId)
                .OnDelete(DeleteBehavior.Restrict); 

                //Neden OnDelete(DeleteBehavior.Restrict) kullandım Islem ilişkilerinde? 
                // Bir hesap silinmeye çalışıldığında, 
                // o hesaba ait işlem geçmişi otomatik silinmesin istiyoruz (finansal kayıtlar kaybolmamalı). 
                // Cascade yerine Restrict koydum, yani ilişkili işlem varsa hesap silinemez 
                // (bu mantığı ileride Service katmanında ele alacağız).

            // Islem - AliciHesap iliskisi
            modelBuilder.Entity<Islem>()
                .HasOne(i => i.AliciHesap)
                .WithMany(h => h.AlinanIslemler)
                .HasForeignKey(i => i.AliciHesapId)
                .OnDelete(DeleteBehavior.Restrict);

            // Benzersiz alanlar
            modelBuilder.Entity<Musteri>()
                .HasIndex(m => m.TcKimlikNo)
                .IsUnique();

            modelBuilder.Entity<Hesap>()
                .HasIndex(h => h.HesapNumarasi)
                .IsUnique();

            modelBuilder.Entity<Hesap>()
                .HasIndex(h => h.Iban)
                .IsUnique();

            // Decimal hassasiyeti (PostgreSQL icin onemli)
            modelBuilder.Entity<Hesap>()
                .Property(h => h.Bakiye)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Islem>()
                .Property(i => i.Tutar)
                .HasPrecision(18, 2);
        }
    }
}