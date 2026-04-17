using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class canbo_hoso
{
    public int MaCanBo { get; set; }

    public int HoSoID { get; set; }

    public DateTime ThoiGian { get; set; }

    public string? TrangThaiDuyet { get; set; }

    public virtual hoso HoSo { get; set; } = null!;

    public virtual canbo MaCanBoNavigation { get; set; } = null!;
}
