namespace bankingapp.API.DTOs.Islem
{
    public class TransferDto
    {
        public int GonderenHesapId { get; set; }
        public string AliciIban { get; set; } = string.Empty;
        public string AliciAdSoyad { get; set; } = string.Empty;
        public decimal Tutar { get; set; }
        public string? Aciklama { get; set; }
    }
}