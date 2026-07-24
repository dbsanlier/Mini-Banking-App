namespace bankingapp.API.DTOs.Musteri
{
    public class MusteriResponseDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string TcKimlikNo { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime OlusturulmaTarihi { get; set; }
    }
}

//Entityi dogrudan APIden gondermiyoruz, "Circular reference" hatasini engellemek icin DTO kullanioz
// DTO, entityden farkli olarak sadece gerekli alanlari iceriyor yani
// MusteriResponseDto, Musteri entitysinin sadece response islemi icin gerekli alanlarini iceriyo
