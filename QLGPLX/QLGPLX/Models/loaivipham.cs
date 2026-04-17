using System;
using System.Collections.Generic;

namespace QLGPLX.Models;

public partial class loaivipham
{
    public int LoaiViPhamID { get; set; }

    public string? TenViPham { get; set; }

    public int? DiemTru { get; set; }

    public decimal? MucPhatTu { get; set; }

    public decimal? MucPhatDen { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<vipham> viphams { get; set; } = new List<vipham>();
}
