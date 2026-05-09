using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.KyThi
{
    public class KyThiDTO
    {
        public int KyThiID { get; set; }

        // DB là Guid? → DTO để Guid (mapper sẽ xử lý null)
        public Guid PublicId { get; set; }

        public string? TenKyThi { get; set; }

        public DateOnly? NgayBatDau { get; set; }
        public DateOnly? NgayKetThuc { get; set; }

        public string? DiaDiem { get; set; }

        public string? MaHang { get; set; }
        public string? TenHang { get; set; }
        
        public int SoLuongToiDa { get; set; }
        public int SoLuongDangKy { get; set; }

    }

    // =========================
    // CREATE DTO
    // =========================
    public class CreateKyThiDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Tên kỳ thi không được để trống")]
        [MaxLength(150, ErrorMessage = "Tên kỳ thi tối đa 150 ký tự")]
        public string? TenKyThi { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateOnly? NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateOnly? NgayKetThuc { get; set; }

        [MaxLength(255)]
        public string? DiaDiem { get; set; }

        [Required(ErrorMessage = "Hạng GPLX không được để trống")]
        public string? MaHang { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Phải lớn hơn 0")]
        public int? SoLuongToiDa { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            if (NgayBatDau.HasValue && NgayKetThuc.HasValue)
            {
                if (NgayKetThuc < NgayBatDau)
                {
                    yield return new ValidationResult(
                        "Ngày kết thúc phải sau ngày bắt đầu",
                        new[] { nameof(NgayKetThuc) });
                }
            }
            if (NgayBatDau < today)
            {
                yield return new ValidationResult(
                    "Ngày bắt đầu không hợp lệ",
                    new[] { nameof(NgayBatDau) });
            }
        }
    }

    // =========================
    // UPDATE DTO
    // =========================
    public class UpdateKyThiDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Tên kỳ thi không được để trống")]
        [MaxLength(150, ErrorMessage = "Tên kỳ thi tối đa 150 ký tự")]
        public string? TenKyThi { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateOnly? NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateOnly? NgayKetThuc { get; set; }

        [MaxLength(255)]
        public string? DiaDiem { get; set; }

        [Required(ErrorMessage = "Hạng GPLX không được để trống")]
        public string? MaHang { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Phải lớn hơn 0")]
        public int? SoLuongToiDa { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            if (NgayBatDau.HasValue && NgayKetThuc.HasValue)
            {
                if (NgayKetThuc < NgayBatDau)
                {
                    yield return new ValidationResult(
                        "Ngày kết thúc phải sau ngày bắt đầu",
                        new[] { nameof(NgayKetThuc) });
                }
            }
            if (NgayBatDau < today)
            {
                yield return new ValidationResult(
                    "Ngày bắt đầu không hợp lệ",
                    new[] { nameof(NgayBatDau) });
            }
        }
    }

    // =========================
    // THÊM HỒ SƠ VÀO KỲ THI
    // =========================
    public class ThemHoSoVaoKyThiDTO
    {
        public int KyThiID { get; set; }
        public List<int> DanhSachHoSoID { get; set; } = new();
    }

    // =========================
    // HỒ SƠ ĐÃ DUYỆT
    // =========================
    public class HoSoDaDuyetDTO
    {
        public int HoSoID { get; set; }

        // DB là Guid? → DTO để Guid cho gọn API
        public Guid PublicId { get; set; }

        public int MaCongDan { get; set; }

        public string? HoTenCongDan { get; set; }
        public string? CCCD { get; set; }

        public string? MaHang { get; set; }
        public string? TenHang { get; set; }

        public DateTime NgayNop { get; set; }

        public bool DaDangKyKyThi { get; set; }
    }
}