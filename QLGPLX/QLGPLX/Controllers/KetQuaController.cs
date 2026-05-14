using Backend.Data;
using Backend.DTO.KetQua;
using Backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CAP_GPLX")]
    public class KetQuaController : ControllerBase
    {
        private readonly IKetQuaService _service;
        private readonly GplxDbContext _context;

        public KetQuaController(IKetQuaService service, GplxDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet("kythi/{kyThiId}")]
        public async Task<IActionResult> GetByKyThi(int kyThiId)
        {
            try
            {
                var result = await _service.GetHoSoKetQuaByKyThiAsync(kyThiId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách kết quả", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetKetQuaByIdAsync(id);
                if (result == null)
                    return NotFound(new { message = "Không tìm thấy kết quả thi" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy kết quả", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateKetQuaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.CreateKetQuaAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.KetQuaID }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo kết quả", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateKetQuaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.UpdateKetQuaAsync(id, dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật kết quả", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteKetQuaAsync(id);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy kết quả thi" });

                return Ok(new { message = "Xóa kết quả thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa kết quả", error = ex.Message });
            }
        }

        // Controllers/HangGiayPhepController.cs - Thêm method này
        [HttpGet("{maHang}/monthi")]
        public async Task<IActionResult> GetMonThiByHang(string maHang)
        {
            try
            {
                var monThi = await _context.HangMonThis
                    .Where(h => h.MaHang == maHang)
                    .Include(h => h.MonThi)
                    .Select(h => new
                    {
                        h.MonThiid,
                        h.MonThi.TenMon,
                        h.DiemDat,
                        h.DiemToiDa
                    })
                    .ToListAsync();

                return Ok(monThi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách môn thi", error = ex.Message });
            }
        }

        // Controllers/KetQuaController.cs - Thêm vào class

        // GET: api/KetQua/{ketQuaId}/chitiet
        [HttpGet("{ketQuaId}/chitiet")]
        public async Task<IActionResult> GetChiTiet(int ketQuaId)
        {
            try
            {
                var result = await _service.GetKetQuaChiTietByKetQuaIdAsync(ketQuaId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy chi tiết kết quả", error = ex.Message });
            }
        }

        // POST: api/KetQua/{ketQuaId}/chitiet
        [HttpPost("{ketQuaId}/chitiet")]
        public async Task<IActionResult> CreateChiTiet(int ketQuaId, [FromBody] CreateKetQuaChiTietDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.CreateKetQuaChiTietAsync(ketQuaId, dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo kết quả chi tiết", error = ex.Message });
            }
        }

        // PUT: api/KetQua/chitiet/{chiTietId}
        [HttpPut("chitiet/{chiTietId}")]
        public async Task<IActionResult> UpdateChiTiet(int chiTietId, [FromBody] UpdateKetQuaChiTietDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.UpdateKetQuaChiTietAsync(chiTietId, dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật kết quả chi tiết", error = ex.Message });
            }
        }

        // DELETE: api/KetQua/chitiet/{chiTietId}
        [HttpDelete("chitiet/{chiTietId}")]
        public async Task<IActionResult> DeleteChiTiet(int chiTietId)
        {
            try
            {
                var result = await _service.DeleteKetQuaChiTietAsync(chiTietId);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy kết quả chi tiết" });

                return Ok(new { message = "Xóa kết quả chi tiết thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa kết quả chi tiết", error = ex.Message });
            }
        }

    }


}
