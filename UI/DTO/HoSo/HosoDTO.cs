namespace UI.DTO.Hoso;

public class HosoDTO
{
    public int HoSoId { get; set; }
    public Guid? PublicId { get; set; }
    public int MaCongDan { get; set; }
    public string MaHang { get; set; } = null!;
    public DateTime? NgayNop { get; set; }
    public string? TrangThai { get; set; }
    public bool? TrangThaiThanhToan { get; set; }
    public string? GhiChu { get; set; }
    
    // Navigation properties - thông tin bổ sung
    public string? TenCongDan { get; set; }
    public string? CCCD { get; set; }
    public string? TenHang { get; set; }
}