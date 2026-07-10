namespace bankingapp.API.Entities
{
    public enum HesapDurumu
    {
        Aktif,
        Kapali
    }

    public class Hesap
    {
        public int Id { get; set; }
        public string HesapNumarasi { get; set; } = string.Empty;
        public string Iban { get; set; } = string.Empty;
        public decimal Bakiye { get; set; }
        public HesapDurumu Durum { get; set; } = HesapDurumu.Aktif;
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int MusteriId { get; set; }
        public Musteri? Musteri { get; set; }

        // Navigation property
        public ICollection<Islem> GonderilenIslemler { get; set; } = new List<Islem>();
        public ICollection<Islem> AlinanIslemler { get; set; } = new List<Islem>();
    }
}