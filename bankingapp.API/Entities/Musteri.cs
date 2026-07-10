namespace bankingapp.API.Entities
{
    public class Musteri
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string TcKimlikNo { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<Hesap> Hesaplar { get; set; } = new List<Hesap>();
    }
}