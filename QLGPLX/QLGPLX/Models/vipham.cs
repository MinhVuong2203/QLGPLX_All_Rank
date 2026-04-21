using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[Table("vipham")]
[Index("GiayPhepId", Name = "GiayPhepID")]
[Index("LoaiViPhamId", Name = "LoaiViPhamID")]
public partial class Vipham
{
    [Key]
    [Column("ViPhamID")]
    public int ViPhamId { get; set; }

    [Column("GiayPhepID")]
    public int? GiayPhepId { get; set; }

    [Column("LoaiViPhamID")]
    public int? LoaiViPhamId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ThoiGianViPham { get; set; }

    [StringLength(255)]
    public string? DiaDiem { get; set; }

    [StringLength(20)]
    public string? BienKiemSoat { get; set; }

    [Precision(18, 2)]
    public decimal? MucPhat { get; set; }

    [StringLength(30)]
    public string? TrangThai { get; set; }

    [StringLength(500)]
    public string? GhiChu { get; set; }

    [ForeignKey("GiayPhepId")]
    [InverseProperty("Viphams")]
    public virtual Giayphep? GiayPhep { get; set; }

    [ForeignKey("LoaiViPhamId")]
    [InverseProperty("Viphams")]
    public virtual Loaivipham? LoaiViPham { get; set; }
}
