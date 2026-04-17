using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class ketquathi
{
    public int KetQuaID { get; set; }

    public int? HoSoID { get; set; }

    public int? KyThiID { get; set; }

    public string? KetQuaTongHop { get; set; }

    public DateTime? NgayKetLuan { get; set; }

    public int? LanThi { get; set; }

    public string? GhiChu { get; set; }

    public virtual hoso? HoSo { get; set; }

    public virtual kythi? KyThi { get; set; }

    public virtual ICollection<ketquachitiet> ketquachitiets { get; set; } = new List<ketquachitiet>();
}
