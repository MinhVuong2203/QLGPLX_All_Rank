using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class giayphep
{
    public int GiayPhepID { get; set; }

    public int? MaCongDan { get; set; }

    public string? MaHang { get; set; }

    public string? SoGiayPhep { get; set; }

    public DateOnly? NgayCap { get; set; }

    public DateOnly? NgayHetHan { get; set; }

    public int? SoDiem { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual congdan? MaCongDanNavigation { get; set; }

    public virtual hanggiayphep? MaHangNavigation { get; set; }

    public virtual ICollection<vipham> viphams { get; set; } = new List<vipham>();
}
