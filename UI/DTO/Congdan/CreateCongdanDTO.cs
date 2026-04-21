using System.ComponentModel.DataAnnotations;
namespace DTO.Congdan;
public class CreateCongdanDTO
{
    [Required]
    public string HoTen { get; set; }

    [Required]
    public DateOnly NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    [Required]
    public string Cccd { get; set; }

    public string? DiaChi { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }

    public string? TinhTrangSucKhoe { get; set; }
    public string? Anh3x4 { get; set; }
}