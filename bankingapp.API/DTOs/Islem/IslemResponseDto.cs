namespace bankingapp.API.DTOs.Islem
{
    public class IslemResponseDto
    {
        public int Id { get; set; }
        public string Tipi { get; set; } = string.Empty;
        public decimal Tutar { get; set; }
        public string? Aciklama { get; set; }
        public DateTime IslemTarihi { get; set; }
        public string? GonderenHesapNumarasi { get; set; }
        public string? AliciHesapNumarasi { get; set; }
    }
}