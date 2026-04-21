using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QLGPLX.Models;

namespace QLGPLX.Data;

public partial class GplxDbContext : DbContext
{
    public GplxDbContext(DbContextOptions<GplxDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Canbo> Canbos { get; set; }

    public virtual DbSet<CanboHoso> CanboHosos { get; set; }

    public virtual DbSet<Chucvu> Chucvus { get; set; }

    public virtual DbSet<Congdan> Congdans { get; set; }

    public virtual DbSet<Giayphep> Giaypheps { get; set; }

    public virtual DbSet<HangMonThi> HangMonThis { get; set; }

    public virtual DbSet<Hanggiayphep> Hanggiaypheps { get; set; }

    public virtual DbSet<Hoso> Hosos { get; set; }

    public virtual DbSet<Ketquachitiet> Ketquachitiets { get; set; }

    public virtual DbSet<Ketquathi> Ketquathis { get; set; }

    public virtual DbSet<Kythi> Kythis { get; set; }

    public virtual DbSet<Lichthi> Lichthis { get; set; }

    public virtual DbSet<Loaivipham> Loaiviphams { get; set; }

    public virtual DbSet<Monthi> Monthis { get; set; }

    public virtual DbSet<Vipham> Viphams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Canbo>(entity =>
        {
            entity.HasKey(e => e.MaCanBo).HasName("PRIMARY");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaChucVuNavigation).WithMany(p => p.Canbos).HasConstraintName("canbo_ibfk_1");
        });

        modelBuilder.Entity<CanboHoso>(entity =>
        {
            entity.HasKey(e => new { e.MaCanBo, e.HoSoId, e.ThoiGian })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.Property(e => e.ThoiGian).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.HoSo).WithMany(p => p.CanboHosos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("canbo_hoso_ibfk_2");

            entity.HasOne(d => d.MaCanBoNavigation).WithMany(p => p.CanboHosos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("canbo_hoso_ibfk_1");
        });

        modelBuilder.Entity<Chucvu>(entity =>
        {
            entity.HasKey(e => e.MaChucVu).HasName("PRIMARY");
        });

        modelBuilder.Entity<Congdan>(entity =>
        {
            entity.HasKey(e => e.MaCongDan).HasName("PRIMARY");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TinhTrangSucKhoe).HasDefaultValueSql("'Khỏe mạnh'");
        });

        modelBuilder.Entity<Giayphep>(entity =>
        {
            entity.HasKey(e => e.GiayPhepId).HasName("PRIMARY");

            entity.Property(e => e.SoDiem).HasDefaultValueSql("'12'");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'Còn hiệu lực'");

            entity.HasOne(d => d.MaCongDanNavigation).WithMany(p => p.Giaypheps).HasConstraintName("giayphep_ibfk_1");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.Giaypheps).HasConstraintName("giayphep_ibfk_2");
        });

        modelBuilder.Entity<HangMonThi>(entity =>
        {
            entity.HasKey(e => new { e.MaHang, e.MonThiid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.HangMonThis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_mon_thi_ibfk_1");

            entity.HasOne(d => d.MonThi).WithMany(p => p.HangMonThis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_mon_thi_ibfk_2");
        });

        modelBuilder.Entity<Hanggiayphep>(entity =>
        {
            entity.HasKey(e => e.MaHang).HasName("PRIMARY");

            entity.Property(e => e.DoTuoiToiThieu).HasDefaultValueSql("'18'");
            entity.Property(e => e.YeuCauThucHanh).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Hoso>(entity =>
        {
            entity.HasKey(e => e.HoSoId).HasName("PRIMARY");

            entity.Property(e => e.NgayNop).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'Đang xử lý'");
            entity.Property(e => e.TrangThaiThanhToan).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.MaCongDanNavigation).WithMany(p => p.Hosos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hoso_ibfk_1");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.Hosos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hoso_ibfk_2");
        });

        modelBuilder.Entity<Ketquachitiet>(entity =>
        {
            entity.HasKey(e => e.ChiTietId).HasName("PRIMARY");

            entity.HasOne(d => d.KetQuaNavigation).WithMany(p => p.Ketquachitiets)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ketquachitiet_ibfk_1");

            entity.HasOne(d => d.MonThi).WithMany(p => p.Ketquachitiets).HasConstraintName("ketquachitiet_ibfk_2");
        });

        modelBuilder.Entity<Ketquathi>(entity =>
        {
            entity.HasKey(e => e.KetQuaId).HasName("PRIMARY");

            entity.Property(e => e.LanThi).HasDefaultValueSql("'1'");
            entity.Property(e => e.NgayKetLuan).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.HoSo).WithMany(p => p.Ketquathis).HasConstraintName("ketquathi_ibfk_1");

            entity.HasOne(d => d.KyThi).WithMany(p => p.Ketquathis).HasConstraintName("ketquathi_ibfk_2");
        });

        modelBuilder.Entity<Kythi>(entity =>
        {
            entity.HasKey(e => e.KyThiId).HasName("PRIMARY");

            entity.Property(e => e.TrangThai).HasDefaultValueSql("'Sắp diễn ra'");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.Kythis).HasConstraintName("kythi_ibfk_1");
        });

        modelBuilder.Entity<Lichthi>(entity =>
        {
            entity.HasKey(e => e.LichThiId).HasName("PRIMARY");

            entity.HasOne(d => d.KyThi).WithMany(p => p.Lichthis).HasConstraintName("lichthi_ibfk_1");

            entity.HasOne(d => d.MonThi).WithMany(p => p.Lichthis).HasConstraintName("lichthi_ibfk_2");
        });

        modelBuilder.Entity<Loaivipham>(entity =>
        {
            entity.HasKey(e => e.LoaiViPhamId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Monthi>(entity =>
        {
            entity.HasKey(e => e.MonThiId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Vipham>(entity =>
        {
            entity.HasKey(e => e.ViPhamId).HasName("PRIMARY");

            entity.Property(e => e.ThoiGianViPham).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'Chưa xử lý'");

            entity.HasOne(d => d.GiayPhep).WithMany(p => p.Viphams).HasConstraintName("vipham_ibfk_1");

            entity.HasOne(d => d.LoaiViPham).WithMany(p => p.Viphams).HasConstraintName("vipham_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
