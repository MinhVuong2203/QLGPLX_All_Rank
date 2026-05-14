namespace Backend.DTO.GiayPhep
{
    public class GiayPhepDTO
    {
        public int GiayPhepId { get; set; }
        public int? MaCongDan { get; set; }
        public string? TenCongDan { get; set; }
        public string? CCCD { get; set; }
        public string? MaHang { get; set; }
        public string? TenHang { get; set; }
        public string? SoGiayPhep { get; set; }
        public DateOnly? NgayCap { get; set; }
        public DateOnly? NgayHetHan { get; set; }
        public int? SoDiem { get; set; }
        public string? TrangThai { get; set; }
        public string? GhiChu { get; set; }
        public string? DiaChi { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string? Anh3x4 { get; set; }
        public string? LoaiXe { get; set; }
    }

    public class GiayPhepCreateDTO
    {
        public int MaCongDan { get; set; }
        public string MaHang { get; set; }
        public string? SoGiayPhep { get; set; }
        public DateOnly NgayCap { get; set; }
        public DateOnly NgayHetHan { get; set; }
        public int SoDiem { get; set; } = 12;
        public string TrangThai { get; set; } = "Còn hiệu lực";
        public string? GhiChu { get; set; }
    }

    public class GiayPhepUpdateDTO
    {
        public string? TrangThai { get; set; }
        public int? SoDiem { get; set; }
        public string? GhiChu { get; set; }
    }

    public class GiayPhepSearchDTO
    {
        public string? SearchTerm { get; set; }
        public string? TrangThai { get; set; }
        public DateOnly? NgayCapFrom { get; set; }
        public DateOnly? NgayCapTo { get; set; }
        public string? SortBy { get; set; } = "NgayCap";
        public string? SortOrder { get; set; } = "desc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedResult<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
