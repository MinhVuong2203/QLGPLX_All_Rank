using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[Table("kythi")]
[Index("MaHang", Name = "MaHang")]
[Index("PublicId", Name = "public_id", IsUnique = true)]
public partial class Kythi
{
    [Key]
    [Column("KyThiID")]
    public int KyThiId { get; set; }

    [Column("public_id")]
    public Guid? PublicId { get; set; }

    [StringLength(150)]
    public string? TenKyThi { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayKetThuc { get; set; }

    [StringLength(255)]
    public string? DiaDiem { get; set; }

    [StringLength(10)]
    public string? MaHang { get; set; }

    public int? SoLuongToiDa { get; set; }

    [StringLength(30)]
    public string? TrangThai { get; set; }

    [InverseProperty("KyThi")]
    public virtual ICollection<Ketquathi> Ketquathis { get; set; } = new List<Ketquathi>();

    [InverseProperty("KyThi")]
    public virtual ICollection<Lichthi> Lichthis { get; set; } = new List<Lichthi>();

    [ForeignKey("MaHang")]
    [InverseProperty("Kythis")]
    public virtual Hanggiayphep? MaHangNavigation { get; set; }
}
