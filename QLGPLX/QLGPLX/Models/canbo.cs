using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class canbo
{
    public int MaCanBo { get; set; }

    public Guid? public_id { get; set; }

    public string? HoTen { get; set; }

    public int? MaChucVu { get; set; }

    public string? Email { get; set; }

    public string? DienThoai { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Anh3x4 { get; set; }

    public bool? TrangThai { get; set; }

    public virtual chucvu? MaChucVuNavigation { get; set; }

    public virtual ICollection<canbo_hoso> canbo_hosos { get; set; } = new List<canbo_hoso>();
}
