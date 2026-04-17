using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class kythi
{
    public int KyThiID { get; set; }

    public Guid? public_id { get; set; }

    public string? TenKyThi { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayKetThuc { get; set; }

    public string? DiaDiem { get; set; }

    public string? MaHang { get; set; }

    public int? SoLuongToiDa { get; set; }

    public string? TrangThai { get; set; }

    public virtual hanggiayphep? MaHangNavigation { get; set; }

    public virtual ICollection<ketquathi> ketquathis { get; set; } = new List<ketquathi>();

    public virtual ICollection<lichthi> lichthis { get; set; } = new List<lichthi>();
}
