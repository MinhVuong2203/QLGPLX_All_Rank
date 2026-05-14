using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; } = string.Empty;
    }
}
