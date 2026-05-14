using Backend.DTO.GiayPhep;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Repository
{
    public class GiayPhepRepository
    {
        private readonly GplxDbContext _context;

        public GiayPhepRepository(GplxDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<GiayPhepDTO>> GetGiayPhepsAsync(GiayPhepSearchDTO search)
        {
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
                query = query.Where(g => g.TrangThai == search.TrangThai);
            }

            // Date filter
            if (search.NgayCapFrom.HasValue)
            {
                var fromDate = search.NgayCapFrom;
                query = query.Where(g => g.NgayCap >= fromDate);
            }

            if (search.NgayCapTo.HasValue)
            {
                var toDate =search.NgayCapTo;
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
                    ? query.OrderBy(g => g.TrangThai)
                    : query.OrderByDescending(g => g.TrangThai),

                _ => query.OrderByDescending(g => g.NgayCap)
            };

            // Paging
            var data = await query
                .Skip((search.PageNumber - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(g => new GiayPhepDTO
                {
                    GiayPhepId = g.GiayPhepId,
                    MaCongDan = g.MaCongDan,
                    TenCongDan = g.MaCongDanNavigation.HoTen,
                    CCCD = g.MaCongDanNavigation.Cccd,
                    MaHang = g.MaHang,
                    TenHang = g.MaHangNavigation.TenHang,
                    SoGiayPhep = g.SoGiayPhep,
                    NgayCap = g.NgayCap,

                    NgayHetHan = g.NgayHetHan,

                    SoDiem = g.SoDiem,
                    TrangThai = g.TrangThai,
                    GhiChu = g.GhiChu,
                    DiaChi = g.MaCongDanNavigation.DiaChi,

                    NgaySinh = g.MaCongDanNavigation.NgaySinh
     
                })
                .ToListAsync();

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
            return await _context.Giaypheps
                .Include(g => g.MaCongDanNavigation)
                .Include(g => g.MaHangNavigation)
                .Where(g => g.GiayPhepId == id)
                .Select(g => new GiayPhepDTO
                {
                    GiayPhepId = g.GiayPhepId,
                    MaCongDan = g.MaCongDan,
                    TenCongDan = g.MaCongDanNavigation.HoTen,
                    CCCD = g.MaCongDanNavigation.Cccd,
                    MaHang = g.MaHang,
                    TenHang = g.MaHangNavigation.TenHang,
                    SoGiayPhep = g.SoGiayPhep,
                    NgayCap = g.NgayCap,
                    NgayHetHan = g.NgayHetHan,
                    SoDiem = g.SoDiem,
                    TrangThai = g.TrangThai,
                    GhiChu = g.GhiChu,
                    DiaChi = g.MaCongDanNavigation.DiaChi,
                    NgaySinh = g.MaCongDanNavigation.NgaySinh,
                    Anh3x4 = g.MaCongDanNavigation.Anh3x4,
                    LoaiXe = g.MaHangNavigation.LoaiXe
                })
                .FirstOrDefaultAsync();
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

        public async Task<object> GetStatisticsAsync()
        {
            return new
            {
                Total = await _context.Giaypheps.CountAsync(),

                ConHieuLuc = await _context.Giaypheps
                    .CountAsync(g => g.TrangThai == "Còn hiệu lực"),

                HetHan = await _context.Giaypheps
                    .CountAsync(g => g.TrangThai == "Hết hạn"),

                BiThuHoi = await _context.Giaypheps
                    .CountAsync(g => g.TrangThai == "Bị thu hồi"),

                SapHetHan = await _context.Giaypheps
                    .CountAsync(g =>
                        g.NgayHetHan.HasValue &&
                        g.NgayHetHan.Value <= DateOnly.FromDateTime(DateTime.Now.AddMonths(3)) &&
                        g.TrangThai == "Còn hiệu lực")
            };
        }
    }
}
