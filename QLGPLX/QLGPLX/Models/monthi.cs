using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class monthi
{
    public int MonThiID { get; set; }

    public string TenMon { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<hang_mon_thi> hang_mon_this { get; set; } = new List<hang_mon_thi>();

    public virtual ICollection<ketquachitiet> ketquachitiets { get; set; } = new List<ketquachitiet>();

    public virtual ICollection<lichthi> lichthis { get; set; } = new List<lichthi>();
}
