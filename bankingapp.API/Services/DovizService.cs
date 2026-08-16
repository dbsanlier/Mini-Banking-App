using System.Text.Json;
using bankingapp.API.DTOs.Doviz;

namespace bankingapp.API.Services
{
    public class DovizService : IDovizService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DovizService> _logger;

        private static readonly (string Kod, string Ad, string Sembol)[] TakipEdilenParalar = new[]
        {
            ("USD", "Amerikan Dolari", "$"),
            ("EUR", "Euro", "€"),
            ("GBP", "Ingiliz Sterlini", "£"),
        };

        public DovizService(HttpClient httpClient, ILogger<DovizService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<DovizKuruDto>> GetKurlarAsync()
        {
            var sonuclar = new List<DovizKuruDto>();

            foreach (var (kod, ad, sembol) in TakipEdilenParalar)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"https://open.er-api.com/v6/latest/{kod}");
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);

                    var tryKuru = doc.RootElement
                        .GetProperty("rates")
                        .GetProperty("TRY")
                        .GetDecimal();

                    sonuclar.Add(new DovizKuruDto
                    {
                        Kod = kod,
                        Ad = ad,
                        Sembol = sembol,
                        TlKarsiligi = tryKuru
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "{Kod} kuru alinamadi.", kod);
                    // Bir para birimi basarisiz olsa bile digerlerini dondurmeye devam et
                }
            }

            return sonuclar;
        }
    }
}