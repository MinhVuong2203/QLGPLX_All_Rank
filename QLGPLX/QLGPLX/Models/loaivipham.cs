using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[Table("loaivipham")]
public partial class Loaivipham
{
    [Key]
    [Column("LoaiViPhamID")]
    public int LoaiViPhamId { get; set; }

    [StringLength(255)]
    public string? TenViPham { get; set; }

    public int? DiemTru { get; set; }

    [Precision(18, 2)]
    public decimal? MucPhatTu { get; set; }

    [Precision(18, 2)]
    public decimal? MucPhatDen { get; set; }

    [StringLength(500)]
    public string? MoTa { get; set; }

    [InverseProperty("LoaiViPham")]
    public virtual ICollection<Vipham> Viphams { get; set; } = new List<Vipham>();
}
