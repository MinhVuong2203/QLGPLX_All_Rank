using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("ketquachitiet")]
[Index("KetQuaId", "MonThiId", Name = "KetQuaID", IsUnique = true)]
[Index("MonThiId", Name = "ketquachitiet_ibfk_2")]
public partial class Ketquachitiet
{
    [Key]
    [Column("ChiTietID")]
    public int ChiTietId { get; set; }

    [Column("KetQuaID")]
    public int KetQuaId { get; set; }

    [Column("MonThiID")]
    public int MonThiId { get; set; }

    public int Diem { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ThoiGianBatDau { get; set; }

    [StringLength(20)]
    public string? KetQua { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("KetQuaId")]
    [InverseProperty("Ketquachitiets")]
    public virtual Ketquathi KetQuaNavigation { get; set; } = null!;

    [ForeignKey("MonThiId")]
    [InverseProperty("Ketquachitiets")]
    public virtual Monthi MonThi { get; set; } = null!;
}
