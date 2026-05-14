using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("chucnang")]
[Index("MaChucNangCode", Name = "MaChucNangCode", IsUnique = true)]
public partial class Chucnang
{
    [Key]
    public int MaChucNang { get; set; }

    [StringLength(100)]
    public string TenChucNang { get; set; } = null!;

    [StringLength(100)]
    public string MaChucNangCode { get; set; } = null!;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public bool? TrangThai { get; set; }

    [ForeignKey("MaChucNang")]
    [InverseProperty("MaChucNangs")]
    public virtual ICollection<Canbo> MaCanBos { get; set; } = new List<Canbo>();
}
