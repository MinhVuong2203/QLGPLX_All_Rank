namespace Backend.DTO.CanBo
{
    public class CanBoResponseDto
    {
        public int MaCanBo { get; set; }
        public Guid PublicId { get; set; }
        public string? HoTen { get; set; }
        public int? MaChucVu { get; set; }
        public string? TenChucVu { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Cccd { get; set; } = string.Empty;
        public string? DienThoai { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? Anh3x4 { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool TrangThai { get; set; }
        public int SoQuyen { get; set; }
    }
}
