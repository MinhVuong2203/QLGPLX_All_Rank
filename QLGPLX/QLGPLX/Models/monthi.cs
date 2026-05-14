using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("monthi")]
public partial class Monthi
{
    [Key]
    [Column("MonThiID")]
    public int MonThiId { get; set; }

    [StringLength(100)]
    public string TenMon { get; set; } = null!;

    [StringLength(255)]
    public string? MoTa { get; set; }

    [InverseProperty("MonThi")]
    public virtual ICollection<HangMonThi> HangMonThis { get; set; } = new List<HangMonThi>();

    [InverseProperty("MonThi")]
    public virtual ICollection<Ketquachitiet> Ketquachitiets { get; set; } = new List<Ketquachitiet>();
}
