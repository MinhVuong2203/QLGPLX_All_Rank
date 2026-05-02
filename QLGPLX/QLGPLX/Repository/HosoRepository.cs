using Microsoft.EntityFrameworkCore;
using QLGPLX.Data;
using QLGPLX.Models;

namespace QLGPLX.Repository;

public class HosoRepository
{
    private readonly GplxDbContext _context;

    public HosoRepository(GplxDbContext context)
    {
        _context = context;
    }

    public async Task<List<Hoso>> GetAllAsync()
    {
        return await _context.Hosos
            .Include(h => h.MaCongDanNavigation)
            .Include(h => h.MaHangNavigation)
            .OrderByDescending(h => h.NgayNop)
            .ToListAsync();
    }

    public async Task<Hoso?> GetByIdAsync(int id)
    {
        return await _context.Hosos
            .Include(h => h.MaCongDanNavigation)
            .Include(h => h.MaHangNavigation)
            .FirstOrDefaultAsync(h => h.HoSoId == id);
    }

    public async Task<Hoso?> GetByPublicIdAsync(Guid publicId)
    {
        return await _context.Hosos
            .Include(h => h.MaCongDanNavigation)
            .Include(h => h.MaHangNavigation)
            .FirstOrDefaultAsync(h => h.PublicId == publicId);
    }

    public async Task<Hoso> CreateAsync(Hoso hoso)
    {
        _context.Hosos.Add(hoso);
        await _context.SaveChangesAsync();
        return hoso;
    }

    public async Task<Hoso?> UpdateAsync(Hoso hoso)
    {
        _context.Entry(hoso).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return hoso;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hoso = await _context.Hosos.FindAsync(id);
        if (hoso == null) return false;

        _context.Hosos.Remove(hoso);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Hosos.AnyAsync(h => h.HoSoId == id);
    }

    public async Task<List<Hoso>> GetByCongDanAsync(int maCongDan)
    {
        return await _context.Hosos
            .Include(h => h.MaCongDanNavigation)
            .Include(h => h.MaHangNavigation)
            .Where(h => h.MaCongDan == maCongDan)
            .OrderByDescending(h => h.NgayNop)
            .ToListAsync();
    }
}