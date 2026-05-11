using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLGPLX.Models;

[PrimaryKey("MaHang", "MonThiid")]
[Table("hang_mon_thi")]
[Index("MonThiid", Name = "mon_thiid")]
public partial class HangMonThi
{
    [Key]
    [Column("ma_hang")]
    [StringLength(10)]
    public string MaHang { get; set; } = null!;

    [Key]
    [Column("mon_thiid")]
    public int MonThiid { get; set; }

    [Column("diem_dat")]
    public int DiemDat { get; set; }

    [Column("diem_toi_da")]
    public int DiemToiDa { get; set; }

    [ForeignKey("MaHang")]
    [InverseProperty("HangMonThis")]
    public virtual Hanggiayphep MaHangNavigation { get; set; } = null!;

    [ForeignKey("MonThiid")]
    [InverseProperty("HangMonThis")]
    public virtual Monthi MonThi { get; set; } = null!;
}
