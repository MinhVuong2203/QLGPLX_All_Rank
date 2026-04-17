using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class vipham
{
    public int ViPhamID { get; set; }

    public int? GiayPhepID { get; set; }

    public int? LoaiViPhamID { get; set; }

    public DateTime? ThoiGianViPham { get; set; }

    public string? DiaDiem { get; set; }

    public string? BienKiemSoat { get; set; }

    public decimal? MucPhat { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual giayphep? GiayPhep { get; set; }

    public virtual loaivipham? LoaiViPham { get; set; }
}
