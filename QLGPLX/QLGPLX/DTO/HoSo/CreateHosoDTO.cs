using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.HoSo;

public class CreateHosoDTO
{
    [Required(ErrorMessage = "Vui lòng chọn công dân")]
    public int MaCongDan { get; set; }
    
    [Required(ErrorMessage = "Vui lòng chọn hạng GPLX")]
    [StringLength(10)]
    public string MaHang { get; set; } = null!;
    
    [StringLength(255)]
    public string? GhiChu { get; set; }

}

public class CreateMultipleHosoDTO
{
    [Required(ErrorMessage = "Vui lòng chọn ít nhất một công dân")]
    public List<int> MaCongDans { get; set; } = new();

    [Required(ErrorMessage = "Vui lòng chọn hạng GPLX")]
    [StringLength(10)]
    public string MaHang { get; set; } = null!;

    [StringLength(255)]
    public string? GhiChu { get; set; }
}
