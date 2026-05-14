using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("hoso")]
[Index("MaCongDan", Name = "MaCongDan")]
[Index("MaHang", Name = "MaHang")]
[Index("PublicId", Name = "public_id", IsUnique = true)]
public partial class Hoso
{
    [Key]
    [Column("HoSoID")]
    public int HoSoId { get; set; }

    [Column("public_id")]
    public Guid? PublicId { get; set; }

    public int MaCongDan { get; set; }

    [StringLength(10)]
    public string MaHang { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NgayNop { get; set; }

    [StringLength(30)]
    public string? TrangThai { get; set; }

    public bool? TrangThaiThanhToan { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("HoSo")]
    public virtual ICollection<Ketquathi> Ketquathis { get; set; } = new List<Ketquathi>();

    [ForeignKey("MaCongDan")]
    [InverseProperty("Hosos")]
    public virtual Congdan MaCongDanNavigation { get; set; } = null!;

    [ForeignKey("MaHang")]
    [InverseProperty("Hosos")]
    public virtual Hanggiayphep MaHangNavigation { get; set; } = null!;
}
