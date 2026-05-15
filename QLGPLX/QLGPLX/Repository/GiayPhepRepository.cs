using Backend.DTO.GiayPhep;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using QRCoder;

namespace Backend.Repository
{
    public class GiayPhepRepository
    {
        private readonly GplxDbContext _context;
        private const string TrangThaiChoDuyet = "Chờ duyệt";
        private const string TrangThaiConHieuLuc = "Còn hiệu lực";
        private const string TrangThaiHetHan = "Hết hạn";
        private const string TrangThaiBiThuHoi = "Bị thu hồi";

        public GiayPhepRepository(GplxDbContext context)
        {
            _context = context;
        }

        private static string GetTrangThaiHienThi(Giayphep giayPhep, DateOnly today)
        {
            if (giayPhep.TrangThai == TrangThaiChoDuyet)
                return TrangThaiChoDuyet;

            if ((giayPhep.SoDiem ?? 0) == 0)
                return TrangThaiBiThuHoi;

            if (giayPhep.NgayHetHan.HasValue && giayPhep.NgayHetHan.Value < today)
                return TrangThaiHetHan;

            return TrangThaiConHieuLuc;
        }

        private static string CreateQrCodeDataUrl(Giayphep giayPhep, string trangThaiHienThi)
        {
            var payload = string.Join(Environment.NewLine, new[]
            {
                "GIẤY PHÉP LÁI XE",
                $"Số GPLX: {giayPhep.SoGiayPhep ?? "-"}",
                $"Họ tên: {giayPhep.MaCongDanNavigation?.HoTen ?? "-"}",
                $"Hạng: {giayPhep.MaHang ?? "-"}",
                $"Ngày cấp: {FormatQrDate(giayPhep.NgayCap)}",
                $"Ngày hết hạn: {FormatQrDate(giayPhep.NgayHetHan)}",
                $"Trạng thái: {trangThaiHienThi}",
                $"Nơi cấp: An Giang"
            });

            using var generator = new QRCodeGenerator();
            using var qrData = generator.CreateQrCode(
                payload,
                QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrData);
            var qrBytes = qrCode.GetGraphic(8);

            return $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
        }

        private static string FormatQrDate(DateOnly? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "-";
        }

        private static GiayPhepDTO ToDto(Giayphep g, DateOnly today)
        {
            var trangThaiHienThi = GetTrangThaiHienThi(g, today);

            return new GiayPhepDTO
            {
                GiayPhepId = g.GiayPhepId,
                MaCongDan = g.MaCongDan,
                TenCongDan = g.MaCongDanNavigation?.HoTen,
                CCCD = g.MaCongDanNavigation?.Cccd,
                MaHang = g.MaHang,
                TenHang = g.MaHangNavigation?.TenHang,
                SoGiayPhep = g.SoGiayPhep,
                NgayCap = g.NgayCap,
                NgayHetHan = g.NgayHetHan,
                SoDiem = g.SoDiem,
                TrangThai = g.TrangThai,
                TrangThaiHienThi = trangThaiHienThi,
                GhiChu = g.GhiChu,
                DiaChi = g.MaCongDanNavigation?.DiaChi,
                NgaySinh = g.MaCongDanNavigation?.NgaySinh,
                Anh3x4 = g.MaCongDanNavigation?.Anh3x4,
                LoaiXe = g.MaHangNavigation?.LoaiXe,
                QrCodeDataUrl = CreateQrCodeDataUrl(g, trangThaiHienThi)
            };
        }

        private static IQueryable<Giayphep> ApplyTrangThaiFilter(
            IQueryable<Giayphep> query,
            string trangThai,
            DateOnly today)
        {
            return trangThai switch
            {
                TrangThaiChoDuyet => query.Where(g => g.TrangThai == TrangThaiChoDuyet),
                TrangThaiBiThuHoi => query.Where(g =>
                    g.TrangThai != TrangThaiChoDuyet &&
                    (g.SoDiem ?? 0) == 0),
                TrangThaiHetHan => query.Where(g =>
                    g.TrangThai != TrangThaiChoDuyet &&
                    (g.SoDiem ?? 0) > 0 &&
                    g.NgayHetHan.HasValue &&
                    g.NgayHetHan.Value < today),
                TrangThaiConHieuLuc => query.Where(g =>
                    g.TrangThai != TrangThaiChoDuyet &&
                    (g.SoDiem ?? 0) > 0 &&
                    (!g.NgayHetHan.HasValue || g.NgayHetHan.Value >= today)),
                _ => query
            };
        }

        public async Task<PagedResult<GiayPhepDTO>> GetGiayPhepsAsync(GiayPhepSearchDTO search)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var query = _context.Giaypheps
                .Include(g => g.MaCongDanNavigation)
                .Include(g => g.MaHangNavigation)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(search.SearchTerm))
            {
                var searchLower = search.SearchTerm.ToLower();

                query = query.Where(g =>
                    g.SoGiayPhep!.ToLower().Contains(searchLower) ||
                    g.MaCongDanNavigation.HoTen!.ToLower().Contains(searchLower) ||
                    g.MaCongDanNavigation.Cccd!.Contains(searchLower)
                );
            }

            // Filter status
            if (!string.IsNullOrEmpty(search.TrangThai))
            {
                query = ApplyTrangThaiFilter(query, search.TrangThai, today);
            }

            // Date filter
            if (search.NgayCapFrom.HasValue)
            {
                var fromDate = search.NgayCapFrom;
                query = query.Where(g => g.NgayCap >= fromDate);
            }

            if (search.NgayCapTo.HasValue)
            {
                var toDate = search.NgayCapTo;
                query = query.Where(g => g.NgayCap <= toDate);
            }

            // Count
            var totalRecords = await query.CountAsync();

            // Sort
            query = search.SortBy?.ToLower() switch
            {
                "hoten" => search.SortOrder == "asc"
                    ? query.OrderBy(g => g.MaCongDanNavigation.HoTen)
                    : query.OrderByDescending(g => g.MaCongDanNavigation.HoTen),

                "sogiayphep" => search.SortOrder == "asc"
                    ? query.OrderBy(g => g.SoGiayPhep)
                    : query.OrderByDescending(g => g.SoGiayPhep),

                "ngaycap" => search.SortOrder == "asc"
                    ? query.OrderBy(g => g.NgayCap)
                    : query.OrderByDescending(g => g.NgayCap),

                "ngayhethan" => search.SortOrder == "asc"
                    ? query.OrderBy(g => g.NgayHetHan)
                    : query.OrderByDescending(g => g.NgayHetHan),

                "trangthai" => search.SortOrder == "asc"
                    ? query.OrderBy(g => g.TrangThai == TrangThaiChoDuyet ? 0 :
                        (g.SoDiem ?? 0) == 0 ? 1 :
                        g.NgayHetHan.HasValue && g.NgayHetHan.Value < today ? 2 : 3)
                    : query.OrderByDescending(g => g.TrangThai == TrangThaiChoDuyet ? 0 :
                        (g.SoDiem ?? 0) == 0 ? 1 :
                        g.NgayHetHan.HasValue && g.NgayHetHan.Value < today ? 2 : 3),

                _ => query.OrderByDescending(g => g.NgayCap)
            };

            // Paging
            var items = await query
                .Skip((search.PageNumber - 1) * search.PageSize)
                .Take(search.PageSize)
                .ToListAsync();

            var data = items.Select(g => ToDto(g, today)).ToList();

            return new PagedResult<GiayPhepDTO>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = search.PageNumber,
                PageSize = search.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)search.PageSize)
            };
        }

        public async Task<GiayPhepDTO?> GetGiayPhepByIdAsync(int id)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var giayPhep = await _context.Giaypheps
                .Include(g => g.MaCongDanNavigation)
                .Include(g => g.MaHangNavigation)
                .Where(g => g.GiayPhepId == id)
                .FirstOrDefaultAsync();

            return giayPhep == null ? null : ToDto(giayPhep, today);
        }

        public async Task<Giayphep> CreateGiayPhepAsync(GiayPhepCreateDTO dto)
        {
            var giayPhep = new Giayphep
            {
                MaCongDan = dto.MaCongDan,
                MaHang = dto.MaHang,
                SoGiayPhep = dto.SoGiayPhep,
                NgayCap = dto.NgayCap,
                NgayHetHan = dto.NgayHetHan,
                SoDiem = dto.SoDiem,
                TrangThai = dto.TrangThai,
                GhiChu = dto.GhiChu
            };

            _context.Giaypheps.Add(giayPhep);

            await _context.SaveChangesAsync();

            return giayPhep;
        }

        public async Task<bool> UpdateGiayPhepAsync(int id, GiayPhepUpdateDTO dto)
        {
            var giayPhep = await _context.Giaypheps.FindAsync(id);

            if (giayPhep == null)
                return false;

            if (dto.TrangThai != null)
                giayPhep.TrangThai = dto.TrangThai;

            if (dto.SoDiem.HasValue)
                giayPhep.SoDiem = dto.SoDiem;

            if (dto.GhiChu != null)
                giayPhep.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<LichSuGiayPhepDTO>> GetLichSuAsync(int giayPhepId)
        {
            return await _context.Lichsugiaypheps
                .Where(l => l.GiayPhepId == giayPhepId)
                .OrderByDescending(l => l.NgayThucHien)
                .Select(l => new LichSuGiayPhepDTO
                {
                    LichSuId = l.LichSuId,
                    GiayPhepId = l.GiayPhepId,
                    LoaiThaoTac = l.LoaiThaoTac,
                    SoGiayPhep = l.SoGiayPhep,
                    NgayCapCu = l.NgayCapCu,
                    NgayHetHanCu = l.NgayHetHanCu,
                    NgayCapMoi = l.NgayCapMoi,
                    NgayHetHanMoi = l.NgayHetHanMoi,
                    LyDo = l.LyDo,
                    NgayThucHien = l.NgayThucHien
                })
                .ToListAsync();
        }

        public async Task<bool> DuyetGiayPhepAsync(int id, GiayPhepActionDTO dto)
        {
            var giayPhep = await _context.Giaypheps.FindAsync(id);

            if (giayPhep == null)
                return false;

            if (giayPhep.TrangThai != TrangThaiChoDuyet)
                throw new InvalidOperationException("Chỉ giấy phép ở trạng thái Chờ duyệt mới được duyệt.");

            giayPhep.TrangThai = TrangThaiConHieuLuc;
            giayPhep.SoDiem = 12;
            giayPhep.GhiChu = dto.GhiChu;

            _context.Lichsugiaypheps.Add(new Lichsugiayphep
            {
                GiayPhepId = giayPhep.GiayPhepId,
                LoaiThaoTac = "DUYET",
                SoGiayPhep = giayPhep.SoGiayPhep ?? string.Empty,
                NgayCapMoi = giayPhep.NgayCap,
                NgayHetHanMoi = giayPhep.NgayHetHan,
                LyDo = dto.GhiChu,
                NgayThucHien = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CapLaiGiayPhepAsync(int id, GiayPhepActionDTO dto)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var giayPhep = await _context.Giaypheps
                .Include(g => g.MaHangNavigation)
                .FirstOrDefaultAsync(g => g.GiayPhepId == id);

            if (giayPhep == null)
                return false;

            var trangThaiHienThi = GetTrangThaiHienThi(giayPhep, today);
            var laThuHoi = trangThaiHienThi == TrangThaiBiThuHoi;
            var laHetHan = trangThaiHienThi == TrangThaiHetHan;

            if (!laThuHoi && !laHetHan)
                throw new InvalidOperationException("Chỉ giấy phép bị thu hồi hoặc hết hạn mới được cấp mới/cấp lại.");

            var ngayCapCu = giayPhep.NgayCap;
            var ngayHetHanCu = giayPhep.NgayHetHan;
            var soNam = giayPhep.MaHangNavigation?.ThoiHanNam ?? 10;

            giayPhep.NgayCap = today;
            giayPhep.NgayHetHan = today.AddYears(soNam);
            giayPhep.SoDiem = 12;
            giayPhep.TrangThai = TrangThaiConHieuLuc;
            giayPhep.GhiChu = dto.GhiChu;

            _context.Lichsugiaypheps.Add(new Lichsugiayphep
            {
                GiayPhepId = giayPhep.GiayPhepId,
                LoaiThaoTac = laThuHoi ? "CAP_MOI" : "CAP_LAI",
                SoGiayPhep = giayPhep.SoGiayPhep ?? string.Empty,
                NgayCapCu = ngayCapCu,
                NgayHetHanCu = ngayHetHanCu,
                NgayCapMoi = giayPhep.NgayCap,
                NgayHetHanMoi = giayPhep.NgayHetHan,
                LyDo = dto.GhiChu,
                NgayThucHien = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetStatisticsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return new
            {
                Total = await _context.Giaypheps.CountAsync(),

                ConHieuLuc = await _context.Giaypheps
                    .CountAsync(g =>
                        g.TrangThai != TrangThaiChoDuyet &&
                        (g.SoDiem ?? 0) > 0 &&
                        (!g.NgayHetHan.HasValue || g.NgayHetHan.Value >= today)),

                HetHan = await _context.Giaypheps
                    .CountAsync(g =>
                        g.TrangThai != TrangThaiChoDuyet &&
                        (g.SoDiem ?? 0) > 0 &&
                        g.NgayHetHan.HasValue &&
                        g.NgayHetHan.Value < today),

                BiThuHoi = await _context.Giaypheps
                    .CountAsync(g =>
                        g.TrangThai != TrangThaiChoDuyet &&
                        (g.SoDiem ?? 0) == 0),

                SapHetHan = await _context.Giaypheps
                    .CountAsync(g =>
                        g.TrangThai != TrangThaiChoDuyet &&
                        (g.SoDiem ?? 0) > 0 &&
                        g.NgayHetHan.HasValue &&
                        g.NgayHetHan.Value <= DateOnly.FromDateTime(DateTime.Now.AddMonths(3)) &&
                        g.NgayHetHan.Value >= today)
            };
        }
    }
}
