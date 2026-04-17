using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class chucvu
{
    public int MaChucVu { get; set; }

    public string? TenChucVu { get; set; }

    public virtual ICollection<canbo> canbos { get; set; } = new List<canbo>();
}
