using Backend.DTO.HoSo;
using Backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace QLGPLX.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "HO_SO")] // QL_HO_SO + DUYET_HO_SO
public class HosoController : ControllerBase
{
    private readonly IHosoService _hosoService;

    public HosoController(IHosoService hosoService)
    {
        _hosoService = hosoService;
    }

    // GET: api/Hoso
   
    [HttpGet]
    public async Task<ActionResult<List<HosoDTO>>> GetAll()
    {
        try
        {
            var hosos = await _hosoService.GetAllAsync();
            return Ok(hosos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy danh sách hồ sơ", error = ex.Message });
        }
    }

    // GET: api/Hoso/5
    //[Authorize(Policy = "HO_SO")]
    [HttpGet("{id}")]
    public async Task<ActionResult<HosoDTO>> GetById(int id)
    {
        try
        {
            var hoso = await _hosoService.GetByIdAsync(id);
            if (hoso == null)
                return NotFound(new { message = "Không tìm thấy hồ sơ" });

            return Ok(hoso);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy hồ sơ", error = ex.Message });
        }
    }

    // GET: api/Hoso/public/guid
    //[Authorize(Policy = "HO_SO")]
    [HttpGet("public/{publicId}")]
    public async Task<ActionResult<HosoDTO>> GetByPublicId(Guid publicId)
    {
        try
        {
            var hoso = await _hosoService.GetByPublicIdAsync(publicId);
            if (hoso == null)
                return NotFound(new { message = "Không tìm thấy hồ sơ" });

            return Ok(hoso);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy hồ sơ", error = ex.Message });
        }
    }

    // GET: api/Hoso/congdan/5
    //[Authorize(Policy = "HO_SO")]
    [HttpGet("congdan/{maCongDan}")]
    public async Task<ActionResult<List<HosoDTO>>> GetByCongDan(int maCongDan)
    {
        try
        {
            var hosos = await _hosoService.GetByCongDanAsync(maCongDan);
            return Ok(hosos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy danh sách hồ sơ", error = ex.Message });
        }
    }

    // GET: api/Hoso/check-exists?maCongDan=1&maHang=A1
    //[Authorize(Policy = "HO_SO")]
    [HttpGet("check-exists")]
    public async Task<ActionResult<bool>> CheckExists([FromQuery] int maCongDan, [FromQuery] string maHang)
    {
        try
        {
            var exists = await _hosoService.ExistsByMaCongDanAndMaHangAsync(maCongDan, maHang);
            return Ok(new { exists });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi kiểm tra hồ sơ", error = ex.Message });
        }
    }

    // POST: api/Hoso
    //[Authorize(Policy = "HO_SO")]
    [HttpPost]
    public async Task<ActionResult<HosoDTO>> Create([FromBody] CreateHosoDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _hosoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.HoSoId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi tạo hồ sơ", error = ex.Message });
        }
    }

    // PUT: api/Hoso/5
    [Authorize(Policy = "DUYET_HO_SO")]
    [HttpPut("{id}")]
    public async Task<ActionResult<HosoDTO>> Update(int id, [FromBody] UpdateHosoDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _hosoService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = "Không tìm thấy hồ sơ" });

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi cập nhật hồ sơ", error = ex.Message });
        }
    }

    // DELETE: api/Hoso/5
    //[Authorize(Policy = "HO_SO")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _hosoService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Không tìm thấy hồ sơ" });

            return Ok(new { message = "Xóa hồ sơ thành công" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi xóa hồ sơ", error = ex.Message });
        }
    }
}