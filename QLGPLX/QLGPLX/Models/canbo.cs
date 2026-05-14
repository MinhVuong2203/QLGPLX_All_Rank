using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("canbo")]
[Index("Cccd", Name = "Cccd", IsUnique = true)]
[Index("Email", Name = "Email", IsUnique = true)]
[Index("MaChucVu", Name = "MaChucVu")]
[Index("Username", Name = "Username", IsUnique = true)]
[Index("PublicId", Name = "public_id", IsUnique = true)]
public partial class Canbo
{
    [Key]
    public int MaCanBo { get; set; }

    [Column("public_id")]
    public Guid PublicId { get; set; }

    [StringLength(100)]
    public string? HoTen { get; set; }

    public int? MaChucVu { get; set; }

    public string Email { get; set; } = null!;

    [StringLength(12)]
    public string Cccd { get; set; } = null!;

    [StringLength(15)]
    public string? DienThoai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [StringLength(256)]
    public string? Anh3x4 { get; set; }

    [StringLength(100)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    public bool? TrangThai { get; set; }

    [ForeignKey("MaChucVu")]
    [InverseProperty("Canbos")]
    public virtual Chucvu? MaChucVuNavigation { get; set; }

    [InverseProperty("MaCanBoNavigation")]
    public virtual ICollection<Passwordresetotp> Passwordresetotps { get; set; } = new List<Passwordresetotp>();

    [ForeignKey("MaCanBo")]
    [InverseProperty("MaCanBos")]
    public virtual ICollection<Chucnang> MaChucNangs { get; set; } = new List<Chucnang>();
}
