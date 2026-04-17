using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class ketquachitiet
{
    public int ChiTietID { get; set; }

    public int? KetQuaID { get; set; }

    public int? MonThiID { get; set; }

    public decimal? Diem { get; set; }

    public DateTime? ThoiGianBatDau { get; set; }

    public string? KetQua { get; set; }

    public string? GhiChu { get; set; }

    public virtual ketquathi? KetQuaNavigation { get; set; }

    public virtual monthi? MonThi { get; set; }
}
