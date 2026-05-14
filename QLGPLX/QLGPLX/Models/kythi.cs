using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

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

    public int? SoLuongDangKy { get; set; }

    [InverseProperty("KyThi")]
    public virtual ICollection<Ketquathi> Ketquathis { get; set; } = new List<Ketquathi>();

    [ForeignKey("MaHang")]
    [InverseProperty("Kythis")]
    public virtual Hanggiayphep? MaHangNavigation { get; set; }
}
