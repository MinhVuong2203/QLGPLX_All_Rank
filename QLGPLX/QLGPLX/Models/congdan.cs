using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("congdan")]
[Index("Cccd", Name = "CCCD", IsUnique = true)]
[Index("Email", Name = "Email", IsUnique = true)]
[Index("SoDienThoai", Name = "SoDienThoai", IsUnique = true)]
[Index("PublicId", Name = "public_id", IsUnique = true)]
public partial class Congdan
{
    [Key]
    public int MaCongDan { get; set; }

    [Column("public_id")]
    public Guid? PublicId { get; set; }

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    public DateOnly NgaySinh { get; set; }

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [Column("CCCD")]
    [StringLength(20)]
    public string Cccd { get; set; } = null!;

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? TinhTrangSucKhoe { get; set; }

    public DateOnly? NgayKhamSucKhoe { get; set; }

    [StringLength(255)]
    public string? GiayKhamSucKhoe { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [StringLength(255)]
    public string? Anh3x4 { get; set; }

    [InverseProperty("MaCongDanNavigation")]
    public virtual ICollection<Giayphep> Giaypheps { get; set; } = new List<Giayphep>();

    [InverseProperty("MaCongDanNavigation")]
    public virtual ICollection<Hoso> Hosos { get; set; } = new List<Hoso>();
}
