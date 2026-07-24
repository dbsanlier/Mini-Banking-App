namespace bankingapp.API.DTOs.Musteri
{
    public class MusteriCreateDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string TcKimlikNo { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

//Entityi dogrudan APIden gondermiyoruz, "Circular reference" hatasini engellemek icin DTO kullanioz
// DTO, entityden farkli olarak sadece gerekli alanlari iceriyor yani
// MusteriCreateDto, Musteri entitysinin sadece olusturma islemi icin gerekli alanlarini iceriyo