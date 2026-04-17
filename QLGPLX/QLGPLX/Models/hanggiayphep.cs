using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class hanggiayphep
{
    public string MaHang { get; set; } = null!;

    public string TenHang { get; set; } = null!;

    public string? LoaiXe { get; set; }

    public int? DoTuoiToiThieu { get; set; }

    public int? ThoiHanNam { get; set; }

    public bool? YeuCauThucHanh { get; set; }

    public string? MoTaChiTiet { get; set; }

    public virtual ICollection<giayphep> giaypheps { get; set; } = new List<giayphep>();

    public virtual ICollection<hang_mon_thi> hang_mon_this { get; set; } = new List<hang_mon_thi>();

    public virtual ICollection<hoso> hosos { get; set; } = new List<hoso>();

    public virtual ICollection<kythi> kythis { get; set; } = new List<kythi>();
}
