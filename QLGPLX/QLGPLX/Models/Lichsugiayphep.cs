using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("lichsugiayphep")]
[Index("GiayPhepId", Name = "GiayPhepID")]
public partial class Lichsugiayphep
{
    [Key]
    [Column("LichSuID")]
    public int LichSuId { get; set; }

    [Column("GiayPhepID")]
    public int GiayPhepId { get; set; }

    [StringLength(30)]
    public string LoaiThaoTac { get; set; } = null!;

    [StringLength(20)]
    public string SoGiayPhep { get; set; } = null!;

    public DateOnly? NgayCapCu { get; set; }

    public DateOnly? NgayHetHanCu { get; set; }

    public DateOnly? NgayCapMoi { get; set; }

    public DateOnly? NgayHetHanMoi { get; set; }

    [StringLength(255)]
    public string? LyDo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayThucHien { get; set; }

    [ForeignKey("GiayPhepId")]
    [InverseProperty("Lichsugiaypheps")]
    public virtual Giayphep GiayPhep { get; set; } = null!;
}
