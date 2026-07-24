namespace bankingapp.API.DTOs.Hesap
{
    public class HesapResponseDto
    {
        public int Id { get; set; }
        public string HesapNumarasi { get; set; } = string.Empty;
        public string Iban { get; set; } = string.Empty;
        public decimal Bakiye { get; set; }
        public string Durum { get; set; } = string.Empty;
        public DateTime OlusturulmaTarihi { get; set; }
        public int MusteriId { get; set; }
        public string MusteriAdSoyad { get; set; } = string.Empty;
    }
}

//burada da ayni sekilde HesapResponseDto,
// Hesap entitysinin sadece response islemi icin gerekli alanlarini iceriyo