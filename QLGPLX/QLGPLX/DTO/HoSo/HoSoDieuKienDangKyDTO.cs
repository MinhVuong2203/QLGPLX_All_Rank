namespace Backend.DTO.HoSo;

public class HoSoDieuKienDangKyDTO
{
    public int? MaCongDan { get; set; }
    public string MaHangDangKy { get; set; } = null!;
    public bool CoYeuCauGiayPhepKhac { get; set; }
    public bool DuDieuKien { get; set; }
    public string ThongBao { get; set; } = null!;
    public List<HoSoDieuKienHangDTO> DieuKiens { get; set; } = new();
}

public class HoSoDieuKienHangDTO
{
    public string HangBatBuocId { get; set; } = null!;
    public string? TenHangBatBuoc { get; set; }
    public int NamToiThieu { get; set; }
    public bool DuDieuKien { get; set; }
    public string? LyDo { get; set; }
    public string? SoGiayPhep { get; set; }
    public DateOnly? NgayCap { get; set; }
    public DateOnly? NgayHetHan { get; set; }
    public int? SoDiem { get; set; }
    public string? TrangThai { get; set; }
}
