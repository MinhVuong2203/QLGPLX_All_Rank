using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[Table("ketquachitiet")]
[Index("KetQuaId", "MonThiId", Name = "KetQuaID", IsUnique = true)]
[Index("MonThiId", Name = "MonThiID")]
public partial class Ketquachitiet
{
    [Key]
    [Column("ChiTietID")]
    public int ChiTietId { get; set; }

    [Column("KetQuaID")]
    public int? KetQuaId { get; set; }

    [Column("MonThiID")]
    public int? MonThiId { get; set; }

    [Precision(5, 2)]
    public decimal? Diem { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ThoiGianBatDau { get; set; }

    [StringLength(20)]
    public string? KetQua { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("KetQuaId")]
    [InverseProperty("Ketquachitiets")]
    public virtual Ketquathi? KetQuaNavigation { get; set; }

    [ForeignKey("MonThiId")]
    [InverseProperty("Ketquachitiets")]
    public virtual Monthi? MonThi { get; set; }
}
