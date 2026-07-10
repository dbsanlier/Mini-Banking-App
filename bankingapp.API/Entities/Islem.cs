namespace bankingapp.API.Entities
{
    public enum IslemTipi
    {
        ParaYatirma,
        ParaCekme,
        Transfer
    }

    public class Islem
    {
        public int Id { get; set; }
        public IslemTipi Tipi { get; set; }
        public decimal Tutar { get; set; }
        public string? Aciklama { get; set; }
        public DateTime IslemTarihi { get; set; } = DateTime.UtcNow;

        // Para yatirma/cekme icin: tek hesap yeterli
        // Transfer icin: hem gonderen hem alici dolu olur
        public int? GonderenHesapId { get; set; }
        public Hesap? GonderenHesap { get; set; }

        public int? AliciHesapId { get; set; }
        public Hesap? AliciHesap { get; set; }
    }
}