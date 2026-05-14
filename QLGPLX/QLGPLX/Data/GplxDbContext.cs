using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class GplxDbContext : DbContext
{
    public GplxDbContext(DbContextOptions<GplxDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Canbo> Canbos { get; set; }

    public virtual DbSet<Chucnang> Chucnangs { get; set; }

    public virtual DbSet<Chucvu> Chucvus { get; set; }

    public virtual DbSet<Congdan> Congdans { get; set; }

    public virtual DbSet<Giayphep> Giaypheps { get; set; }

    public virtual DbSet<HangMonThi> HangMonThis { get; set; }

    public virtual DbSet<Hanggiayphep> Hanggiaypheps { get; set; }

    public virtual DbSet<Hoso> Hosos { get; set; }

    public virtual DbSet<Ketquachitiet> Ketquachitiets { get; set; }

    public virtual DbSet<Ketquathi> Ketquathis { get; set; }

    public virtual DbSet<Kythi> Kythis { get; set; }

    public virtual DbSet<Lichsugiayphep> Lichsugiaypheps { get; set; }

    public virtual DbSet<Monthi> Monthis { get; set; }

    public virtual DbSet<Passwordresetotp> Passwordresetotps { get; set; }

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

            entity.HasMany(d => d.MaChucNangs).WithMany(p => p.MaCanBos)
                .UsingEntity<Dictionary<string, object>>(
                    "Phanquyencanbo",
                    r => r.HasOne<Chucnang>().WithMany()
                        .HasForeignKey("MaChucNang")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("phanquyencanbo_ibfk_2"),
                    l => l.HasOne<Canbo>().WithMany()
                        .HasForeignKey("MaCanBo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("phanquyencanbo_ibfk_1"),
                    j =>
                    {
                        j.HasKey("MaCanBo", "MaChucNang")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("phanquyencanbo");
                        j.HasIndex(new[] { "MaChucNang" }, "MaChucNang");
                    });
        });

        modelBuilder.Entity<Chucnang>(entity =>
        {
            entity.HasKey(e => e.MaChucNang).HasName("PRIMARY");

            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
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

            entity.HasOne(d => d.KetQuaNavigation).WithMany(p => p.Ketquachitiets).HasConstraintName("ketquachitiet_ibfk_1");

            entity.HasOne(d => d.MonThi).WithMany(p => p.Ketquachitiets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ketquachitiet_ibfk_2");
        });

        modelBuilder.Entity<Ketquathi>(entity =>
        {
            entity.HasKey(e => e.KetQuaId).HasName("PRIMARY");

            entity.Property(e => e.LanThi).HasDefaultValueSql("'1'");
            entity.Property(e => e.NgayKetLuan).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.HoSo).WithMany(p => p.Ketquathis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ketquathi_ibfk_1");

            entity.HasOne(d => d.KyThi).WithMany(p => p.Ketquathis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ketquathi_ibfk_2");
        });

        modelBuilder.Entity<Kythi>(entity =>
        {
            entity.HasKey(e => e.KyThiId).HasName("PRIMARY");

            entity.Property(e => e.SoLuongDangKy).HasDefaultValueSql("'0'");
            entity.Property(e => e.SoLuongToiDa).HasDefaultValueSql("'100'");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.Kythis).HasConstraintName("kythi_ibfk_1");
        });

        modelBuilder.Entity<Lichsugiayphep>(entity =>
        {
            entity.HasKey(e => e.LichSuId).HasName("PRIMARY");

            entity.Property(e => e.NgayThucHien).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.GiayPhep).WithMany(p => p.Lichsugiaypheps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lichsugiayphep_ibfk_1");
        });

        modelBuilder.Entity<Monthi>(entity =>
        {
            entity.HasKey(e => e.MonThiId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Passwordresetotp>(entity =>
        {
            entity.HasKey(e => e.Otpid).HasName("PRIMARY");

            entity.Property(e => e.IsUsed).HasDefaultValueSql("'0'");
            entity.Property(e => e.NgayTao).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.MaCanBoNavigation).WithMany(p => p.Passwordresetotps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("passwordresetotp_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
