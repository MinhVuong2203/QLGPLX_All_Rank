using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class hoso
{
    public int HoSoID { get; set; }

    public Guid? public_id { get; set; }

    public int MaCongDan { get; set; }

    public string MaHang { get; set; } = null!;

    public DateTime? NgayNop { get; set; }

    public string? TrangThai { get; set; }

    public bool? TrangThaiThanhToan { get; set; }

    public string? GhiChu { get; set; }

    public virtual congdan MaCongDanNavigation { get; set; } = null!;

    public virtual hanggiayphep MaHangNavigation { get; set; } = null!;

    public virtual ICollection<canbo_hoso> canbo_hosos { get; set; } = new List<canbo_hoso>();

    public virtual ICollection<ketquathi> ketquathis { get; set; } = new List<ketquathi>();
}
