using Backend.DTO.Congdan;
using Backend.Service.Interface;
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
            try
            {
                await _congdanService.Create(dto, anh3x4, giayKham);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                    return Conflict(ex.Message); 

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham)
        {
            try
            {
                await _congdanService.Update(id, dto, anh3x4, giayKham);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                    return Conflict(ex.Message);

                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _congdanService.Delete(id);
            return Ok();
        }

    }
}
