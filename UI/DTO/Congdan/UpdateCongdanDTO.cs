namespace DTO.Congdan;
public class UpdateCongdanDTO
{
    public string HoTen { get; set; }
    public DateOnly NgaySinh { get; set; }
    public string? GioiTinh { get; set; }

    public string? DiaChi { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }

    public string? TinhTrangSucKhoe { get; set; }
    public string? Anh3x4 { get; set; }
}