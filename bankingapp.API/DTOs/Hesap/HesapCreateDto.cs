namespace bankingapp.API.DTOs.Hesap
{
    public class HesapCreateDto
    {
        public int MusteriId { get; set; }
        public decimal BaslangicBakiyesi { get; set; } = 0;
    }
}

//burada da ayni sekilde HesapCreateDto, 
// Hesap entitysinin sadece olusturma islemi icin gerekli alanlarini iceriyo