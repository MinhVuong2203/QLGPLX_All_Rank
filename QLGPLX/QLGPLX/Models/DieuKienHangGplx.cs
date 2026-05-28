using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("dieu_kien_hang_gplx")]
[Index("HangBatBuocId", Name = "hang_bat_buoc_id")]
[Index("HangDangKyId", Name = "hang_dang_ky_id")]
public partial class DieuKienHangGplx
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("hang_dang_ky_id")]
    [StringLength(10)]
    public string HangDangKyId { get; set; } = null!;

    [Column("hang_bat_buoc_id")]
    [StringLength(10)]
    public string HangBatBuocId { get; set; } = null!;

    [Column("nam_toi_thieu")]
    public int NamToiThieu { get; set; }

    [ForeignKey("HangBatBuocId")]
    [InverseProperty("DieuKienHangBatBuocs")]
    public virtual Hanggiayphep HangBatBuoc { get; set; } = null!;

    [ForeignKey("HangDangKyId")]
    [InverseProperty("DieuKienHangDangKys")]
    public virtual Hanggiayphep HangDangKy { get; set; } = null!;
}
