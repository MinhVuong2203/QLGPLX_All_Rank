using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class hang_mon_thi
{
    public string ma_hang { get; set; } = null!;

    public int mon_thiid { get; set; }

    public decimal diem_dat { get; set; }

    public virtual hanggiayphep ma_hangNavigation { get; set; } = null!;

    public virtual monthi mon_thi { get; set; } = null!;
}
