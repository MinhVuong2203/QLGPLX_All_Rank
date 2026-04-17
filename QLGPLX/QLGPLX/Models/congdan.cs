using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class congdan
{
    public int MaCongDan { get; set; }

    public Guid? public_id { get; set; }

    public string HoTen { get; set; } = null!;

    public DateOnly NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string CCCD { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? TinhTrangSucKhoe { get; set; }

    public DateOnly? NgayKhamSucKhoe { get; set; }

    public string? GiayKhamSucKhoe { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? Anh3x4 { get; set; }

    public virtual ICollection<giayphep> giaypheps { get; set; } = new List<giayphep>();

    public virtual ICollection<hoso> hosos { get; set; } = new List<hoso>();
}
