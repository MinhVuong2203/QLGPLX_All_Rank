using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.HoSo;
public class UpdateHosoDTO
{
    [Required(ErrorMessage = "Vui lòng chọn hạng GPLX")]
    [StringLength(10)]
    public string MaHang { get; set; } = null!;

    [StringLength(255)]
    public string? GhiChu { get; set; }
    public bool? TrangThaiThanhToan { get; set; }

    [StringLength(30)]
    public string? TrangThai { get; set; } = null!;
}
