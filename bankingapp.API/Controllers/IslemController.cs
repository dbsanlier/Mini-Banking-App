using Microsoft.AspNetCore.Mvc;
using bankingapp.API.DTOs.Islem;
using bankingapp.API.Services;

namespace bankingapp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IslemController : ControllerBase
    {
        private readonly IIslemService _islemService;

        public IslemController(IIslemService islemService)
        {
            _islemService = islemService;
        }

        // GET: api/islem/hesap/5
        [HttpGet("hesap/{hesapId}")]
        public async Task<ActionResult<List<IslemResponseDto>>> GetByHesapId(int hesapId)
        {
            var islemler = await _islemService.GetByHesapIdAsync(hesapId);
            return Ok(islemler);
        }

        // POST: api/islem/para-yatir
        [HttpPost("para-yatir")]
        public async Task<ActionResult<IslemResponseDto>> ParaYatir([FromBody] ParaYatirmaDto dto)
        {
            try
            {
                var islem = await _islemService.ParaYatirAsync(dto);
                return Ok(islem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/islem/para-cek
        [HttpPost("para-cek")]
        public async Task<ActionResult<IslemResponseDto>> ParaCek([FromBody] ParaCekmeDto dto)
        {
            try
            {
                var islem = await _islemService.ParaCekAsync(dto);
                return Ok(islem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/islem/transfer
        [HttpPost("transfer")]
        public async Task<ActionResult<IslemResponseDto>> Transfer([FromBody] TransferDto dto)
        {
            try
            {
                var islem = await _islemService.TransferYapAsync(dto);
                return Ok(islem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}