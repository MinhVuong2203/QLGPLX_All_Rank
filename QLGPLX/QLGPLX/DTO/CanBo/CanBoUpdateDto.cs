using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.CanBo
{
    public class CanBoUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        public int MaChucVu { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "CCCD phải gồm đúng 12 chữ số")]
        public string Cccd { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? DienThoai { get; set; }

        [MaxLength(256)]
        public string? Anh3x4 { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        public string? Password { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}
