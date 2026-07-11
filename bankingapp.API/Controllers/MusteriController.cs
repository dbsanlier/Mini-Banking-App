using Microsoft.AspNetCore.Mvc;
using bankingapp.API.DTOs.Musteri;
using bankingapp.API.Services;

namespace bankingapp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusteriController : ControllerBase
    {
        private readonly IMusteriService _musteriService;

        public MusteriController(IMusteriService musteriService)
        {
            _musteriService = musteriService;
        }

        // GET: api/musteri
        [HttpGet]
        public async Task<ActionResult<List<MusteriResponseDto>>> GetAll()
        {
            var musteriler = await _musteriService.GetAllAsync();
            return Ok(musteriler);
        }

        // GET: api/musteri/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusteriResponseDto>> GetById(int id)
        {
            var musteri = await _musteriService.GetByIdAsync(id);
            if (musteri is null)
            {
                return NotFound(new { message = $"ID {id} ile musteri bulunamadi." });
            }
            return Ok(musteri);
        }

        // GET: api/musteri/search?term=ahmet
        [HttpGet("search")]
        public async Task<ActionResult<List<MusteriResponseDto>>> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "Arama terimi bos olamaz." });
            }
            var sonuclar = await _musteriService.SearchAsync(term);
            return Ok(sonuclar);
        }

        // POST: api/musteri
        [HttpPost]
        public async Task<ActionResult<MusteriResponseDto>> Create([FromBody] MusteriCreateDto dto)
        {
            try
            {
                var yeniMusteri = await _musteriService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = yeniMusteri.Id }, yeniMusteri);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/musteri/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MusteriUpdateDto dto)
        {
            var basarili = await _musteriService.UpdateAsync(id, dto);
            if (!basarili)
            {
                return NotFound(new { message = $"ID {id} ile musteri bulunamadi." });
            }
            return NoContent();
        }

        // DELETE: api/musteri/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var basarili = await _musteriService.DeleteAsync(id);
            if (!basarili)
            {
                return NotFound(new { message = $"ID {id} ile musteri bulunamadi." });
            }
            return NoContent();
        }
    }
}