using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Repository
{
    public class KyThiRepository
    {
        private readonly GplxDbContext _context;

        public KyThiRepository(GplxDbContext context)
        {
            _context = context;
        }

        public async Task<List<Kythi>> GetAllAsync()
        {
            return await _context.Kythis
                .Include(k => k.MaHangNavigation)
                .OrderByDescending(k => k.NgayBatDau)
                .ToListAsync();
        }

        public async Task<Kythi> GetByIdAsync(int id)
        {
            return await _context.Kythis
                .Include(k => k.MaHangNavigation)
                .FirstOrDefaultAsync(k => k.KyThiId == id);
        }

        public async Task<Kythi> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Kythis
                .Include(k => k.MaHangNavigation)
                .FirstOrDefaultAsync(k => k.PublicId == publicId);
        }

        public async Task<Kythi> CreateAsync(Kythi kyThi)
        {
            kyThi.PublicId = Guid.NewGuid();
            await _context.Kythis.AddAsync(kyThi);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(kyThi.KyThiId);
        }

        public async Task<Kythi> UpdateAsync(Kythi kyThi)
        {
            _context.Kythis.Update(kyThi);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(kyThi.KyThiId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var kyThi = await GetByIdAsync(id);
            if (kyThi == null) return false;

            _context.Kythis.Remove(kyThi);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Hoso>> GetHoSoDaDuyetAsync(string maHang)
        {
            return await _context.Hosos
                .Include(h => h.MaCongDanNavigation)
                .Include(h => h.MaHangNavigation)
                .Where(h => h.TrangThai == "Đã duyệt" &&
                           (string.IsNullOrEmpty(maHang) || h.MaHang == maHang) &&
                           !_context.Ketquathis.Any(kq =>
                               kq.HoSoId == h.HoSoId &&
                               kq.KyThi.MaHang == h.MaHang))
                .OrderByDescending(h => h.NgayNop)
                .ToListAsync();
        }

        public async Task<List<Hoso>> GetHoSoTrongKyThiAsync(int kyThiId)
        {
            return await _context.Ketquathis
                .Where(k => k.KyThiId == kyThiId)
                .Include(k => k.HoSo) // include HoSo trước
                    .ThenInclude(h => h.MaCongDanNavigation)
                .Include(k => k.HoSo)
                    .ThenInclude(h => h.MaHangNavigation)
                .Select(k => k.HoSo)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Hoso>> GetHoSoByIdsAsync(List<int> hoSoIds)
        {
            return await _context.Hosos
                .Where(h => hoSoIds.Contains(h.HoSoId))
                .Include(h => h.MaCongDanNavigation)
                .Include(h => h.MaHangNavigation)
                .ToListAsync();
        }

        public async Task<List<Hoso>> GetHoSoDaCoKyThiCungHangAsync(
            int kyThiId,
            string maHang,
            List<int> hoSoIds)
        {
            return await _context.Hosos
                .Include(h => h.MaCongDanNavigation)
                .Where(h =>
                    hoSoIds.Contains(h.HoSoId) &&
                    _context.Ketquathis.Any(kq =>
                        kq.HoSoId == h.HoSoId &&
                        kq.KyThiId != kyThiId &&
                        kq.KyThi.MaHang == maHang))
                .ToListAsync();
        }

        public async Task<bool> ThemHoSoVaoKyThiAsync(int kyThiId, List<int> danhSachHoSoId)
        {
            var kyThi = await GetByIdAsync(kyThiId);
            if (kyThi == null) return false;

            foreach (var hoSoId in danhSachHoSoId)
            {
                // Kiểm tra xem hồ sơ đã được thêm vào kỳ thi chưa
                var daTonTai = await _context.Ketquathis
                    .AnyAsync(k => k.HoSoId == hoSoId && k.KyThiId == kyThiId);

                if (!daTonTai)
                {
                    var ketQua = new Ketquathi
                    {
                        HoSoId = hoSoId,
                        KyThiId = kyThiId,
                        KetQuaTongHop = "Chưa thi",
                        LanThi = 1,
                        NgayKetLuan = DateTime.Now
                    };

                    await _context.Ketquathis.AddAsync(ketQua);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> XoaHoSoKhoiKyThiAsync(int kyThiId, int hoSoId)
        {
            var daCoGiayPhep = await _context.Hosos
                .Where(h => h.HoSoId == hoSoId)
                .AnyAsync(h => _context.Giaypheps.Any(g =>
                    g.MaCongDan == h.MaCongDan &&
                    g.MaHang == h.MaHang));

            if (daCoGiayPhep)
            {
                throw new InvalidOperationException("Không thể xóa hồ sơ khỏi kỳ thi vì công dân đã được cấp giấy phép lái xe.");
            }

            var ketQua = await _context.Ketquathis
                .FirstOrDefaultAsync(k => k.KyThiId == kyThiId && k.HoSoId == hoSoId);

            if (ketQua == null) return false;

            _context.Ketquathis.Remove(ketQua);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
