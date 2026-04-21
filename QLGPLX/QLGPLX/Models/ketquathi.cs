using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[Table("ketquathi")]
[Index("HoSoId", "KyThiId", "LanThi", Name = "HoSoID", IsUnique = true)]
[Index("KyThiId", Name = "KyThiID")]
public partial class Ketquathi
{
    [Key]
    [Column("KetQuaID")]
    public int KetQuaId { get; set; }

    [Column("HoSoID")]
    public int? HoSoId { get; set; }

    [Column("KyThiID")]
    public int? KyThiId { get; set; }

    [StringLength(20)]
    public string? KetQuaTongHop { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayKetLuan { get; set; }

    public int? LanThi { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("HoSoId")]
    [InverseProperty("Ketquathis")]
    public virtual Hoso? HoSo { get; set; }

    [InverseProperty("KetQuaNavigation")]
    public virtual ICollection<Ketquachitiet> Ketquachitiets { get; set; } = new List<Ketquachitiet>();

    [ForeignKey("KyThiId")]
    [InverseProperty("Ketquathis")]
    public virtual Kythi? KyThi { get; set; }
}
