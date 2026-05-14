namespace Backend.DTO.Auth
{
    public class AuthCanBoDto
    {
        public Guid PublicId { get; set; }

        public string? HoTen { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string? Anh3x4 { get; set; }

        public int? MaChucVu { get; set; }

        public string? TenChucVu { get; set; }

        public List<string> Quyen { get; set; } = new();
    }
}
