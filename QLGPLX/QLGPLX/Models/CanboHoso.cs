using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[PrimaryKey("MaCanBo", "HoSoId", "ThoiGian")]
[Table("canbo_hoso")]
[Index("HoSoId", Name = "HoSoID")]
public partial class CanboHoso
{
    [Key]
    public int MaCanBo { get; set; }

    [Key]
    [Column("HoSoID")]
    public int HoSoId { get; set; }

    [Key]
    [Column(TypeName = "datetime")]
    public DateTime ThoiGian { get; set; }

    [StringLength(50)]
    public string? TrangThaiDuyet { get; set; }

    [ForeignKey("HoSoId")]
    [InverseProperty("CanboHosos")]
    public virtual Hoso HoSo { get; set; } = null!;

    [ForeignKey("MaCanBo")]
    [InverseProperty("CanboHosos")]
    public virtual Canbo MaCanBoNavigation { get; set; } = null!;
}
