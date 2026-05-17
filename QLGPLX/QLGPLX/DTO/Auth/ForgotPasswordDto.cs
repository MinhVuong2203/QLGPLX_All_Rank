using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.Auth;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập CCCD")]
    [StringLength(12, MinimumLength = 12, ErrorMessage = "CCCD phải gồm 12 chữ số")]
    [RegularExpression(@"^\d{12}$", ErrorMessage = "CCCD phải gồm 12 chữ số")]
    public string Cccd { get; set; } = string.Empty;
}
