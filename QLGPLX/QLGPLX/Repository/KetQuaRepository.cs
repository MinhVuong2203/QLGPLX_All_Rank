using Microsoft.EntityFrameworkCore;
using QLGPLX.Data;
using QLGPLX.Models;

namespace Backend.Repository
{
    public class KetQuaRepository
    {
        private readonly GplxDbContext _context;

        public KetQuaRepository(GplxDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hoso>> GetHoSoByKyThiAsync(int kyThiId)
        {
            // Lấy danh sách hồ sơ có kết quả thi trong kỳ thi
            var hoSoIds = await _context.Ketquathis
                .Where(k => k.KyThiId == kyThiId)
                .Select(k => k.HoSoId)
                .Distinct()
                .ToListAsync();

            return await _context.Hosos
                .Where(h => hoSoIds.Contains(h.HoSoId))
                .Include(h => h.MaCongDanNavigation)
                .Include(h => h.MaHangNavigation)
                .ToListAsync();
        }

        public async Task<List<Ketquathi>> GetKetQuaByHoSoAndKyThiAsync(int hoSoId, int kyThiId)
        {
            return await _context.Ketquathis
                .Where(k => k.HoSoId == hoSoId && k.KyThiId == kyThiId)
                .OrderBy(k => k.LanThi)
                .ToListAsync();
        }

        public async Task<Ketquathi> GetKetQuaByIdAsync(int ketQuaId)
        {
            return await _context.Ketquathis
                .FirstOrDefaultAsync(k => k.KetQuaId == ketQuaId);
        }

        public async Task<List<Ketquachitiet>> GetKetQuaChiTietByKetQuaIdAsync(int ketQuaId)
        {
            return await _context.Ketquachitiets
                .Where(c => c.KetQuaId == ketQuaId)
                .Include(c => c.MonThi)
                .ToListAsync();
        }

        public async Task<List<HangMonThi>> GetMonThiByHangAsync(string maHang)
        {
            return await _context.HangMonThis
                .Where(h => h.MaHang == maHang)
                .Include(h => h.MonThi)
                .ToListAsync();
        }

        public async Task<Ketquathi> CreateKetQuaAsync(Ketquathi ketQua)
        {
            _context.Ketquathis.Add(ketQua);
            await _context.SaveChangesAsync();
            return ketQua;
        }

        public async Task<Ketquachitiet> CreateKetQuaChiTietAsync(Ketquachitiet chiTiet)
        {
            _context.Ketquachitiets.Add(chiTiet);
            await _context.SaveChangesAsync();
            return chiTiet;
        }

        public async Task UpdateKetQuaAsync(Ketquathi ketQua)
        {
            _context.Ketquathis.Update(ketQua);
        }

        public async Task UpdateKetQuaChiTietAsync(Ketquachitiet chiTiet)
        {
            _context.Ketquachitiets.Update(chiTiet);
        }

        public async Task DeleteKetQuaAsync(Ketquathi ketQua)
        {
            _context.Ketquathis.Remove(ketQua);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Repository/Implement/KetQuaRepository.cs - Thêm vào class
        public async Task<Ketquachitiet> GetKetQuaChiTietByIdAsync(int chiTietId)
        {
            return await _context.Ketquachitiets
                .Include(c => c.MonThi)
                .FirstOrDefaultAsync(c => c.ChiTietId == chiTietId);
        }

        public async Task<Kythi> GetKyThiByIdAsync(int kyThiId)
        {
            return await _context.Kythis.FindAsync(kyThiId);
        }

        public async Task<Hoso> GetHoSoByIdAsync(int hoSoId)
        {
            return await _context.Hosos
                .Include(h => h.MaHangNavigation)
                .FirstOrDefaultAsync(h => h.HoSoId == hoSoId);
        }

        public void DeleteKetQuaChiTietAsync(Ketquachitiet chiTiet)
        {
            _context.Ketquachitiets.Remove(chiTiet);
        }

    }
}
