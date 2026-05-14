using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("giayphep")]
[Index("MaCongDan", Name = "MaCongDan")]
[Index("MaHang", Name = "MaHang")]
[Index("SoGiayPhep", Name = "SoGiayPhep", IsUnique = true)]
public partial class Giayphep
{
    [Key]
    [Column("GiayPhepID")]
    public int GiayPhepId { get; set; }

    public int? MaCongDan { get; set; }

    [StringLength(10)]
    public string? MaHang { get; set; }

    [StringLength(20)]
    public string? SoGiayPhep { get; set; }

    public DateOnly? NgayCap { get; set; }

    public DateOnly? NgayHetHan { get; set; }

    public int? SoDiem { get; set; }

    [StringLength(30)]
    public string? TrangThai { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("GiayPhep")]
    public virtual ICollection<Lichsugiayphep> Lichsugiaypheps { get; set; } = new List<Lichsugiayphep>();

    [ForeignKey("MaCongDan")]
    [InverseProperty("Giaypheps")]
    public virtual Congdan? MaCongDanNavigation { get; set; }

    [ForeignKey("MaHang")]
    [InverseProperty("Giaypheps")]
    public virtual Hanggiayphep? MaHangNavigation { get; set; }
}
