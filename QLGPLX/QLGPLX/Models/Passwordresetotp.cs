using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("passwordresetotp")]
[Index("MaCanBo", Name = "MaCanBo")]
public partial class Passwordresetotp
{
    [Key]
    [Column("OTPID")]
    public int Otpid { get; set; }

    public int MaCanBo { get; set; }

    [Column("OTPCode")]
    [StringLength(10)]
    public string Otpcode { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ExpiredAt { get; set; }

    public bool? IsUsed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [ForeignKey("MaCanBo")]
    [InverseProperty("Passwordresetotps")]
    public virtual Canbo MaCanBoNavigation { get; set; } = null!;
}
