using Backend.DTO.GiayPhep;
using Backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CAP_GPLX")]
    public class GiayPhepController : ControllerBase
    {
        private readonly IGiayPhepService _service;

        public GiayPhepController(IGiayPhepService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetGiayPheps([FromQuery] GiayPhepSearchDTO search)
        {
            var result = await _service.GetGiayPhepsAsync(search);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGiayPhep(int id)
        {
            var result = await _service.GetGiayPhepByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGiayPhep(GiayPhepCreateDTO dto)
        {
            var result = await _service.CreateGiayPhepAsync(dto);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGiayPhep(int id, GiayPhepUpdateDTO dto)
        {
            var updated = await _service.UpdateGiayPhepAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _service.GetStatisticsAsync();

            return Ok(result);
        }
    }
}
