using Backend.DTO.KyThi;
using Backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "QUAN_LY_KY_THI")]
    public class KyThiController : ControllerBase
    {
        private readonly IKyThiService _service;

        public KyThiController(IKyThiService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllKyThiAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách kỳ thi", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetKyThiByIdAsync(id);
                if (result == null)
                    return NotFound(new { message = "Không tìm thấy kỳ thi" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin kỳ thi", error = ex.Message });
            }
        }

        [HttpGet("public/{publicId}")]
        public async Task<IActionResult> GetByPublicId(Guid publicId)
        {
            try
            {
                var result = await _service.GetKyThiByPublicIdAsync(publicId);
                if (result == null)
                    return NotFound(new { message = "Không tìm thấy kỳ thi" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin kỳ thi", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateKyThiDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.CreateKyThiAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.KyThiID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo kỳ thi", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateKyThiDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.UpdateKyThiAsync(id, dto);
                if (result == null)
                    return NotFound(new { message = "Không tìm thấy kỳ thi" });

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật kỳ thi", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteKyThiAsync(id);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy kỳ thi" });

                return Ok(new { message = "Xóa kỳ thi thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa kỳ thi", error = ex.Message });
            }
        }

        [HttpGet("hoso-da-duyet")]
        public async Task<IActionResult> GetHoSoDaDuyet([FromQuery] string maHang = null)
        {
            try
            {
                var result = await _service.GetHoSoDaDuyetAsync(maHang);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách hồ sơ đã duyệt", error = ex.Message });
            }
        }

        [HttpGet("{kyThiId}/hoso")]
        public async Task<IActionResult> GetHoSoTrongKyThi(int kyThiId)
        {
            try
            {
                var result = await _service.GetHoSoTrongKyThiAsync(kyThiId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách hồ sơ trong kỳ thi", error = ex.Message });
            }
        }

        [HttpPost("them-hoso")]
        public async Task<IActionResult> ThemHoSoVaoKyThi([FromBody] ThemHoSoVaoKyThiDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.ThemHoSoVaoKyThiAsync(dto);

                if (!result)
                    return BadRequest(new { message = "Không thể thêm hồ sơ vào kỳ thi" });

                return Ok(new { message = "Thêm hồ sơ vào kỳ thi thành công" });
            }
            catch (DbUpdateException ex) // bắt đúng lỗi từ EF
            {
                var msg = ex.InnerException?.Message ?? ex.Message;

                //  map lỗi từ trigger
                if (msg.Contains("Ky thi da du so luong"))
                {
                    return BadRequest(new
                    {
                        message = "Kỳ thi đã đủ số lượng, không thể thêm thí sinh"
                    });
                }

                return StatusCode(500, new
                {
                    message = "Lỗi khi lưu dữ liệu",
                    error = msg
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi thêm hồ sơ vào kỳ thi",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{kyThiId}/hoso/{hoSoId}")]
        public async Task<IActionResult> XoaHoSoKhoiKyThi(int kyThiId, int hoSoId)
        {
            try
            {
                var result = await _service.XoaHoSoKhoiKyThiAsync(kyThiId, hoSoId);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy hồ sơ trong kỳ thi" });

                return Ok(new { message = "Xóa hồ sơ khỏi kỳ thi thành công" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa hồ sơ khỏi kỳ thi", error = ex.Message });
            }
        }
    }
}
