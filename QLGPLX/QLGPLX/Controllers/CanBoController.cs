using Backend.DTO.CanBo;
using Backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "QUAN_LY_CAN_BO")]
    public class CanBoController : ControllerBase
    {
        private readonly ICanBoService _canBoService;

        public CanBoController(ICanBoService canBoService)
        {
            _canBoService = canBoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? keyword,
            [FromQuery] bool? trangThai
        )
        {
            var result = await _canBoService.GetAllAsync(keyword, trangThai);
            return Ok(result);
        }

        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetByPublicId(Guid publicId)
        {
            var result = await _canBoService.GetByPublicIdAsync(publicId);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy cán bộ"
                });
            }

            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CanBoCreateDto dto, IFormFile? anh3x4)
        {
            try
            {
                await _canBoService.CreateAsync(dto, anh3x4);

                return Ok(new
                {
                    message = "Thêm cán bộ thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{publicId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(Guid publicId,[FromForm] CanBoUpdateDto dto, IFormFile? anh3x4
)
        {
            try
            {
                var success = await _canBoService.UpdateAsync(publicId, dto, anh3x4);

                if (!success)
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy cán bộ"
                    });
                }

                return Ok(new
                {
                    message = "Cập nhật cán bộ thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{publicId}")]
        public async Task<IActionResult> Delete(Guid publicId)
        {
            var success = await _canBoService.DeleteAsync(publicId);

            if (!success)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy cán bộ"
                });
            }

            return Ok(new
            {
                message = "Đã ngưng hoạt động cán bộ"
            });
        }

        [HttpPatch("{publicId}/trang-thai")]
        public async Task<IActionResult> ChangeStatus(
            Guid publicId,
            [FromBody] DoiTrangThaiCanBoDto dto
        )
        {
            var success = await _canBoService.ChangeStatusAsync(publicId, dto.TrangThai);

            if (!success)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy cán bộ"
                });
            }

            return Ok(new
            {
                message = dto.TrangThai
                    ? "Đã mở khóa cán bộ"
                    : "Đã khóa cán bộ"
            });
        }

        [HttpGet("chuc-vu")]
        public async Task<IActionResult> GetChucVu()
        {
            var result = await _canBoService.GetChucVuAsync();
            return Ok(result);
        }

        [HttpGet("{publicId}/quyen")]
        public async Task<IActionResult> GetQuyen(Guid publicId)
        {
            var result = await _canBoService.GetQuyenByCanBoAsync(publicId);
            return Ok(result);
        }

        [HttpPut("{publicId}/quyen")]
        public async Task<IActionResult> UpdateQuyen(
            Guid publicId,
            [FromBody] PhanQuyenCanBoDto dto
        )
        {
            var success = await _canBoService.UpdateQuyenAsync(publicId, dto);

            if (!success)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy cán bộ"
                });
            }

            return Ok(new
            {
                message = "Cập nhật quyền cán bộ thành công"
            });
        }
    }
}
