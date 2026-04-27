namespace DTO.Congdan;
public class CongdanDTO
{
    public Guid? PublicId { get; set; }
    public string HoTen { get; set; }
    public DateOnly NgaySinh { get; set; }
    public string? GioiTinh { get; set; }

    public string Cccd { get; set; }

    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }
    public string? DiaChi { get; set; }

    public string? TinhTrangSucKhoe { get; set; }
    public DateOnly? NgayKhamSucKhoe { get; set; }
    public string? GiayKhamSucKhoe { get; set; }
    public string? Anh3x4 { get; set; }
    public DateTime? NgayTao { get; set; }

}