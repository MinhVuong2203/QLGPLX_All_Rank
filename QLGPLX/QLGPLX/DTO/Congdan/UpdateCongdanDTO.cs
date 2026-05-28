using System.ComponentModel.DataAnnotations;
using Backend.Utils;
namespace Backend.DTO.Congdan;
public class UpdateCongdanDTO : IValidatableObject
{
    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string HoTen { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateOnly NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    // Không có cccd ở đây nữa

    public string? DiaChi { get; set; }

    [RegularExpression(@"^0\d{9}$", ErrorMessage = "SĐT phải 10 số và bắt đầu bằng 0")]
    public string? SoDienThoai { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    public string? TinhTrangSucKhoe { get; set; }
    public string? GiayKhamSucKhoe { get; set; }
    public DateOnly? NgayKhamSucKhoe { get; set; }
    public string? Anh3x4 { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var today = VietnamTime.TodayDate;
        if (NgaySinh.Year < 1900)
        {
            yield return new ValidationResult(
                "Ngày sinh phải sau năm 1900",
                new[] { nameof(NgaySinh) });
        }

        if (NgaySinh > today)
        {
            yield return new ValidationResult(
                "Ngày sinh không hợp lệ",
                new[] { nameof(NgaySinh) });
        }

        if (NgayKhamSucKhoe.HasValue)
        {
            // Ngày khám > ngày sinh
            if (NgayKhamSucKhoe <= NgaySinh)
            {
                yield return new ValidationResult(
                    "Ngày khám phải lớn hơn ngày sinh",
                    new[] { nameof(NgayKhamSucKhoe) });
            }

            //Ngày khám không được lớn hơn hiện tại
            if (NgayKhamSucKhoe > today)
            {
                yield return new ValidationResult(
                    "Ngày khám không hợp lệ",
                    new[] { nameof(NgayKhamSucKhoe) });
            }
        }
    }
}
