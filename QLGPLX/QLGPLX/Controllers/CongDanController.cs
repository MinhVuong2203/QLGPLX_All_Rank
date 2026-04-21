using DTO.Congdan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QLGPLX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongDanController : ControllerBase
    {
        private readonly ICongDanService _congdanService;

        public CongDanController(ICongDanService service)
        {
            _congdanService = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_congdanService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var data = _congdanService.GetById(id); 
            return data == null ? NotFound() : Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham)
        {
            await _congdanService.Create(dto, anh3x4, giayKham);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCongdanDTO dto)
        {
            await _congdanService.Update(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _congdanService.Delete(id);
            return Ok();
        }

    }
}
