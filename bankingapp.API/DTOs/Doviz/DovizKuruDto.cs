namespace bankingapp.API.DTOs.Doviz
{
    public class DovizKuruDto
    {
        public string Kod { get; set; } = string.Empty;      // USD, EUR, GBP
        public string Ad { get; set; } = string.Empty;        // Amerikan Dolari
        public string Sembol { get; set; } = string.Empty;    // $
        public decimal TlKarsiligi { get; set; }               // 1 birim = kac TL
    }
}