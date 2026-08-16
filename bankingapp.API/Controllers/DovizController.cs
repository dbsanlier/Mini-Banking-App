using Microsoft.AspNetCore.Mvc;
using bankingapp.API.DTOs.Doviz;
using bankingapp.API.Services;

namespace bankingapp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DovizController : ControllerBase
    {
        private readonly IDovizService _dovizService;

        public DovizController(IDovizService dovizService)
        {
            _dovizService = dovizService;
        }

        // GET: api/doviz/kurlar
        [HttpGet("kurlar")]
        public async Task<ActionResult<List<DovizKuruDto>>> GetKurlar()
        {
            var kurlar = await _dovizService.GetKurlarAsync();
            return Ok(kurlar);
        }
    }
}