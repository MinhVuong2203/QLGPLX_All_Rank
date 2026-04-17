using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class lichthi
{
    public int LichThiID { get; set; }

    public int? KyThiID { get; set; }

    public int? MonThiID { get; set; }

    public DateTime? ThoiGian { get; set; }

    public virtual kythi? KyThi { get; set; }

    public virtual monthi? MonThi { get; set; }
}
