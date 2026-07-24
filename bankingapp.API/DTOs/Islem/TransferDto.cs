namespace bankingapp.API.DTOs.Islem
{
    public class TransferDto
    {
        public int GonderenHesapId { get; set; }
        public int AliciHesapId { get; set; }
        public decimal Tutar { get; set; }
        public string? Aciklama { get; set; }
    }
}