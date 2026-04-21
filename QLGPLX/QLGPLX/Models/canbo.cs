using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[Table("canbo")]
[Index("MaChucVu", Name = "MaChucVu")]
[Index("Username", Name = "Username", IsUnique = true)]
[Index("PublicId", Name = "public_id", IsUnique = true)]
public partial class Canbo
{
    [Key]
    public int MaCanBo { get; set; }

    [Column("public_id")]
    public Guid? PublicId { get; set; }

    [StringLength(100)]
    public string? HoTen { get; set; }

    public int? MaChucVu { get; set; }

    [StringLength(120)]
    public string? Email { get; set; }

    [StringLength(15)]
    public string? DienThoai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [StringLength(100)]
    public string? Username { get; set; }

    [StringLength(100)]
    public string? Password { get; set; }

    [StringLength(256)]
    public string? Anh3x4 { get; set; }

    public bool? TrangThai { get; set; }

    [InverseProperty("MaCanBoNavigation")]
    public virtual ICollection<CanboHoso> CanboHosos { get; set; } = new List<CanboHoso>();

    [ForeignKey("MaChucVu")]
    [InverseProperty("Canbos")]
    public virtual Chucvu? MaChucVuNavigation { get; set; }
}
