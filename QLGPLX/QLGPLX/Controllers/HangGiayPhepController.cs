using Backend.DTO.HangGiayPhep;
using Backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;


namespace QLGPLX.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HangGiayPhepController : ControllerBase
{
    private readonly IHangGiayPhepService _service;

    public HangGiayPhepController(IHangGiayPhepService service)
    {
        _service = service;
    }

    // GET: api/HangGiayPhep
    [HttpGet]
    public async Task<ActionResult<List<HangGiayPhepDTO>>> GetAll()
    {
        try
        {
            var hangs = await _service.GetAllAsync();
            return Ok(hangs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy danh sách hạng GPLX", error = ex.Message });
        }
    }

    // GET: api/HangGiayPhep/A1
    [HttpGet("{maHang}")]
    public async Task<ActionResult<HangGiayPhepDTO>> GetById(string maHang)
    {
        try
        {
            var hang = await _service.GetByIdAsync(maHang);
            if (hang == null)
                return NotFound(new { message = "Không tìm thấy hạng GPLX" });

            return Ok(hang);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy hạng GPLX", error = ex.Message });
        }
    }
}