namespace bankingapp.API.DTOs.Islem
{
    public class ParaCekmeDto
    {
        public int HesapId { get; set; }
        public decimal Tutar { get; set; }
        public string? Aciklama { get; set; }
    }
}