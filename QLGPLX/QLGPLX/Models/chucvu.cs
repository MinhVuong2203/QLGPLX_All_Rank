using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Table("chucvu")]
[Index("TenChucVu", Name = "TenChucVu", IsUnique = true)]
public partial class Chucvu
{
    [Key]
    public int MaChucVu { get; set; }

    [StringLength(50)]
    public string? TenChucVu { get; set; }

    [InverseProperty("MaChucVuNavigation")]
    public virtual ICollection<Canbo> Canbos { get; set; } = new List<Canbo>();
}
