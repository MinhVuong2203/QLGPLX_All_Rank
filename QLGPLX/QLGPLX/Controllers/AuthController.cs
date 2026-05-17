using Backend.DTO.Auth;
using Backend.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.LoginAsync(dto);

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authService.ForgotPasswordAsync(dto);

            return Ok(new
            {
                message = "Nếu email tồn tại trong hệ thống, mã OTP đã được gửi và có hiệu lực trong 10 phút"
            });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.ResetPasswordAsync(dto);

                return Ok(new
                {
                    message = "Đặt lại mật khẩu thành công"
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

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var publicIdClaim = User.FindFirst("publicId")?.Value;

            if (string.IsNullOrWhiteSpace(publicIdClaim))
            {
                return Unauthorized(new
                {
                    message = "Token không hợp lệ"
                });
            }

            var publicId = Guid.Parse(publicIdClaim);

            var result = await _authService.GetMeAsync(publicId);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Không tìm thấy tài khoản"
                });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new
            {
                message = "Đăng xuất thành công"
            });
        }

        [HttpGet("debug-claims")]
        public IActionResult DebugClaims()
        {
            return Ok(User.Claims.Select(c => new
            {
                type = c.Type,
                value = c.Value
            }));
        }
    }
}
