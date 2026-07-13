using Microsoft.AspNetCore.Mvc;
using bankingapp.API.DTOs.Hesap;
using bankingapp.API.Services;

namespace bankingapp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HesapController : ControllerBase
    {
        private readonly IHesapService _hesapService;

        public HesapController(IHesapService hesapService)
        {
            _hesapService = hesapService;
        }

        // GET: api/hesap
        [HttpGet]
        public async Task<ActionResult<List<HesapResponseDto>>> GetAll()
        {
            var hesaplar = await _hesapService.GetAllAsync();
            return Ok(hesaplar);
        }

        // GET: api/hesap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HesapResponseDto>> GetById(int id)
        {
            var hesap = await _hesapService.GetByIdAsync(id);
            if (hesap is null)
            {
                return NotFound(new { message = $"ID {id} ile hesap bulunamadi." });
            }
            return Ok(hesap);
        }

        // GET: api/hesap/musteri/3
        [HttpGet("musteri/{musteriId}")]
        public async Task<ActionResult<List<HesapResponseDto>>> GetByMusteriId(int musteriId)
        {
            var hesaplar = await _hesapService.GetByMusteriIdAsync(musteriId);
            return Ok(hesaplar);
        }

        // POST: api/hesap
        [HttpPost]
        public async Task<ActionResult<HesapResponseDto>> Create([FromBody] HesapCreateDto dto)
        {
            try
            {
                var yeniHesap = await _hesapService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = yeniHesap.Id }, yeniHesap);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/hesap/5/kapat
        [HttpPut("{id}/kapat")]
        public async Task<IActionResult> Kapat(int id)
        {
            try
            {
                var basarili = await _hesapService.KapatAsync(id);
                if (!basarili)
                {
                    return NotFound(new { message = $"ID {id} ile hesap bulunamadi." });
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}