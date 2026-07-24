namespace bankingapp.API.DTOs.Islem
{
    public class ParaYatirmaDto
    {
        public int HesapId { get; set; }
        public decimal Tutar { get; set; }
        public string? Aciklama { get; set; }
    }
}