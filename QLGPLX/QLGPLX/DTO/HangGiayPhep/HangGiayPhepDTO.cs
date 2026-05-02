namespace Backend.DTO.HangGiayPhep;

public class HangGiayPhepDTO
{
    public string MaHang { get; set; } = null!;
    public string TenHang { get; set; } = null!;
    public string? LoaiXe { get; set; }
    public int? DoTuoiToiThieu { get; set; }
    public int? ThoiHanNam { get; set; }
    public bool? YeuCauThucHanh { get; set; }
    public string? MoTaChiTiet { get; set; }
}