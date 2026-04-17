using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QLGPLX.Models;

namespace QLGPLX.Data;

public partial class QlgplxContext : DbContext
{
    public QlgplxContext(DbContextOptions<QlgplxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<canbo> canbos { get; set; }

    public virtual DbSet<canbo_hoso> canbo_hosos { get; set; }

    public virtual DbSet<chucvu> chucvus { get; set; }

    public virtual DbSet<congdan> congdans { get; set; }

    public virtual DbSet<giayphep> giaypheps { get; set; }

    public virtual DbSet<hang_mon_thi> hang_mon_this { get; set; }

    public virtual DbSet<hanggiayphep> hanggiaypheps { get; set; }

    public virtual DbSet<hoso> hosos { get; set; }

    public virtual DbSet<ketquachitiet> ketquachitiets { get; set; }

    public virtual DbSet<ketquathi> ketquathis { get; set; }

    public virtual DbSet<kythi> kythis { get; set; }

    public virtual DbSet<lichthi> lichthis { get; set; }

    public virtual DbSet<loaivipham> loaiviphams { get; set; }

    public virtual DbSet<monthi> monthis { get; set; }

    public virtual DbSet<vipham> viphams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<canbo>(entity =>
        {
            entity.HasKey(e => e.MaCanBo).HasName("PRIMARY");

            entity.ToTable("canbo");

            entity.HasIndex(e => e.MaChucVu, "MaChucVu");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.HasIndex(e => e.public_id, "public_id").IsUnique();

            entity.Property(e => e.Anh3x4).HasMaxLength(256);
            entity.Property(e => e.DienThoai).HasMaxLength(15);
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.MaChucVuNavigation).WithMany(p => p.canbos)
                .HasForeignKey(d => d.MaChucVu)
                .HasConstraintName("canbo_ibfk_1");
        });

        modelBuilder.Entity<canbo_hoso>(entity =>
        {
            entity.HasKey(e => new { e.MaCanBo, e.HoSoID, e.ThoiGian })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("canbo_hoso");

            entity.HasIndex(e => e.HoSoID, "HoSoID");

            entity.Property(e => e.ThoiGian)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThaiDuyet).HasMaxLength(50);

            entity.HasOne(d => d.HoSo).WithMany(p => p.canbo_hosos)
                .HasForeignKey(d => d.HoSoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("canbo_hoso_ibfk_2");

            entity.HasOne(d => d.MaCanBoNavigation).WithMany(p => p.canbo_hosos)
                .HasForeignKey(d => d.MaCanBo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("canbo_hoso_ibfk_1");
        });

        modelBuilder.Entity<chucvu>(entity =>
        {
            entity.HasKey(e => e.MaChucVu).HasName("PRIMARY");

            entity.ToTable("chucvu");

            entity.HasIndex(e => e.TenChucVu, "TenChucVu").IsUnique();

            entity.Property(e => e.TenChucVu).HasMaxLength(50);
        });

        modelBuilder.Entity<congdan>(entity =>
        {
            entity.HasKey(e => e.MaCongDan).HasName("PRIMARY");

            entity.ToTable("congdan");

            entity.HasIndex(e => e.CCCD, "CCCD").IsUnique();

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.SoDienThoai, "SoDienThoai").IsUnique();

            entity.HasIndex(e => e.public_id, "public_id").IsUnique();

            entity.Property(e => e.Anh3x4).HasMaxLength(255);
            entity.Property(e => e.CCCD).HasMaxLength(20);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GiayKhamSucKhoe).HasMaxLength(255);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TinhTrangSucKhoe)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Khỏe mạnh'");
        });

        modelBuilder.Entity<giayphep>(entity =>
        {
            entity.HasKey(e => e.GiayPhepID).HasName("PRIMARY");

            entity.ToTable("giayphep");

            entity.HasIndex(e => e.MaCongDan, "MaCongDan");

            entity.HasIndex(e => e.MaHang, "MaHang");

            entity.HasIndex(e => e.SoGiayPhep, "SoGiayPhep").IsUnique();

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.SoDiem).HasDefaultValueSql("'12'");
            entity.Property(e => e.SoGiayPhep).HasMaxLength(20);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Còn hiệu lực'");

            entity.HasOne(d => d.MaCongDanNavigation).WithMany(p => p.giaypheps)
                .HasForeignKey(d => d.MaCongDan)
                .HasConstraintName("giayphep_ibfk_1");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.giaypheps)
                .HasForeignKey(d => d.MaHang)
                .HasConstraintName("giayphep_ibfk_2");
        });

        modelBuilder.Entity<hang_mon_thi>(entity =>
        {
            entity.HasKey(e => new { e.ma_hang, e.mon_thiid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("hang_mon_thi");

            entity.HasIndex(e => e.mon_thiid, "mon_thiid");

            entity.Property(e => e.ma_hang).HasMaxLength(10);
            entity.Property(e => e.diem_dat).HasPrecision(5, 2);

            entity.HasOne(d => d.ma_hangNavigation).WithMany(p => p.hang_mon_this)
                .HasForeignKey(d => d.ma_hang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_mon_thi_ibfk_1");

            entity.HasOne(d => d.mon_thi).WithMany(p => p.hang_mon_this)
                .HasForeignKey(d => d.mon_thiid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_mon_thi_ibfk_2");
        });

        modelBuilder.Entity<hanggiayphep>(entity =>
        {
            entity.HasKey(e => e.MaHang).HasName("PRIMARY");

            entity.ToTable("hanggiayphep");

            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.DoTuoiToiThieu).HasDefaultValueSql("'18'");
            entity.Property(e => e.LoaiXe).HasMaxLength(50);
            entity.Property(e => e.MoTaChiTiet).HasColumnType("text");
            entity.Property(e => e.TenHang).HasMaxLength(50);
            entity.Property(e => e.YeuCauThucHanh).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<hoso>(entity =>
        {
            entity.HasKey(e => e.HoSoID).HasName("PRIMARY");

            entity.ToTable("hoso");

            entity.HasIndex(e => e.MaCongDan, "MaCongDan");

            entity.HasIndex(e => e.MaHang, "MaHang");

            entity.HasIndex(e => e.public_id, "public_id").IsUnique();

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.NgayNop)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Đang xử lý'");
            entity.Property(e => e.TrangThaiThanhToan).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.MaCongDanNavigation).WithMany(p => p.hosos)
                .HasForeignKey(d => d.MaCongDan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hoso_ibfk_1");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.hosos)
                .HasForeignKey(d => d.MaHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hoso_ibfk_2");
        });

        modelBuilder.Entity<ketquachitiet>(entity =>
        {
            entity.HasKey(e => e.ChiTietID).HasName("PRIMARY");

            entity.ToTable("ketquachitiet");

            entity.HasIndex(e => new { e.KetQuaID, e.MonThiID }, "KetQuaID").IsUnique();

            entity.HasIndex(e => e.MonThiID, "MonThiID");

            entity.Property(e => e.Diem).HasPrecision(5, 2);
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.KetQua).HasMaxLength(20);
            entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");

            entity.HasOne(d => d.KetQuaNavigation).WithMany(p => p.ketquachitiets)
                .HasForeignKey(d => d.KetQuaID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ketquachitiet_ibfk_1");

            entity.HasOne(d => d.MonThi).WithMany(p => p.ketquachitiets)
                .HasForeignKey(d => d.MonThiID)
                .HasConstraintName("ketquachitiet_ibfk_2");
        });

        modelBuilder.Entity<ketquathi>(entity =>
        {
            entity.HasKey(e => e.KetQuaID).HasName("PRIMARY");

            entity.ToTable("ketquathi");

            entity.HasIndex(e => new { e.HoSoID, e.KyThiID, e.LanThi }, "HoSoID").IsUnique();

            entity.HasIndex(e => e.KyThiID, "KyThiID");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.KetQuaTongHop).HasMaxLength(20);
            entity.Property(e => e.LanThi).HasDefaultValueSql("'1'");
            entity.Property(e => e.NgayKetLuan)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.HoSo).WithMany(p => p.ketquathis)
                .HasForeignKey(d => d.HoSoID)
                .HasConstraintName("ketquathi_ibfk_1");

            entity.HasOne(d => d.KyThi).WithMany(p => p.ketquathis)
                .HasForeignKey(d => d.KyThiID)
                .HasConstraintName("ketquathi_ibfk_2");
        });

        modelBuilder.Entity<kythi>(entity =>
        {
            entity.HasKey(e => e.KyThiID).HasName("PRIMARY");

            entity.ToTable("kythi");

            entity.HasIndex(e => e.MaHang, "MaHang");

            entity.HasIndex(e => e.public_id, "public_id").IsUnique();

            entity.Property(e => e.DiaDiem).HasMaxLength(255);
            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.TenKyThi).HasMaxLength(150);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Sắp diễn ra'");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.kythis)
                .HasForeignKey(d => d.MaHang)
                .HasConstraintName("kythi_ibfk_1");
        });

        modelBuilder.Entity<lichthi>(entity =>
        {
            entity.HasKey(e => e.LichThiID).HasName("PRIMARY");

            entity.ToTable("lichthi");

            entity.HasIndex(e => e.KyThiID, "KyThiID");

            entity.HasIndex(e => e.MonThiID, "MonThiID");

            entity.Property(e => e.ThoiGian).HasColumnType("datetime");

            entity.HasOne(d => d.KyThi).WithMany(p => p.lichthis)
                .HasForeignKey(d => d.KyThiID)
                .HasConstraintName("lichthi_ibfk_1");

            entity.HasOne(d => d.MonThi).WithMany(p => p.lichthis)
                .HasForeignKey(d => d.MonThiID)
                .HasConstraintName("lichthi_ibfk_2");
        });

        modelBuilder.Entity<loaivipham>(entity =>
        {
            entity.HasKey(e => e.LoaiViPhamID).HasName("PRIMARY");

            entity.ToTable("loaivipham");

            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.MucPhatDen).HasPrecision(18, 2);
            entity.Property(e => e.MucPhatTu).HasPrecision(18, 2);
            entity.Property(e => e.TenViPham).HasMaxLength(255);
        });

        modelBuilder.Entity<monthi>(entity =>
        {
            entity.HasKey(e => e.MonThiID).HasName("PRIMARY");

            entity.ToTable("monthi");

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenMon).HasMaxLength(100);
        });

        modelBuilder.Entity<vipham>(entity =>
        {
            entity.HasKey(e => e.ViPhamID).HasName("PRIMARY");

            entity.ToTable("vipham");

            entity.HasIndex(e => e.GiayPhepID, "GiayPhepID");

            entity.HasIndex(e => e.LoaiViPhamID, "LoaiViPhamID");

            entity.Property(e => e.BienKiemSoat).HasMaxLength(20);
            entity.Property(e => e.DiaDiem).HasMaxLength(255);
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.MucPhat).HasPrecision(18, 2);
            entity.Property(e => e.ThoiGianViPham)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Chưa xử lý'");

            entity.HasOne(d => d.GiayPhep).WithMany(p => p.viphams)
                .HasForeignKey(d => d.GiayPhepID)
                .HasConstraintName("vipham_ibfk_1");

            entity.HasOne(d => d.LoaiViPham).WithMany(p => p.viphams)
                .HasForeignKey(d => d.LoaiViPhamID)
                .HasConstraintName("vipham_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
