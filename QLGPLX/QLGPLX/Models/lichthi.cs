using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[Table("lichthi")]
[Index("KyThiId", Name = "KyThiID")]
[Index("MonThiId", Name = "MonThiID")]
public partial class Lichthi
{
    [Key]
    [Column("LichThiID")]
    public int LichThiId { get; set; }

    [Column("KyThiID")]
    public int? KyThiId { get; set; }

    [Column("MonThiID")]
    public int? MonThiId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ThoiGian { get; set; }

    [ForeignKey("KyThiId")]
    [InverseProperty("Lichthis")]
    public virtual Kythi? KyThi { get; set; }

    [ForeignKey("MonThiId")]
    [InverseProperty("Lichthis")]
    public virtual Monthi? MonThi { get; set; }
}
