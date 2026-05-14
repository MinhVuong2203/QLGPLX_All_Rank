using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("hanggiayphep")]
public partial class Hanggiayphep
{
    [Key]
    [StringLength(10)]
    public string MaHang { get; set; } = null!;

    [StringLength(50)]
    public string TenHang { get; set; } = null!;

    [StringLength(255)]
    public string? LoaiXe { get; set; }

    public int? DoTuoiToiThieu { get; set; }

    public int? ThoiHanNam { get; set; }

    public bool? YeuCauThucHanh { get; set; }

    [Column(TypeName = "text")]
    public string? MoTaChiTiet { get; set; }

    [InverseProperty("MaHangNavigation")]
    public virtual ICollection<Giayphep> Giaypheps { get; set; } = new List<Giayphep>();

    [InverseProperty("MaHangNavigation")]
    public virtual ICollection<HangMonThi> HangMonThis { get; set; } = new List<HangMonThi>();

    [InverseProperty("MaHangNavigation")]
    public virtual ICollection<Hoso> Hosos { get; set; } = new List<Hoso>();

    [InverseProperty("MaHangNavigation")]
    public virtual ICollection<Kythi> Kythis { get; set; } = new List<Kythi>();
}
