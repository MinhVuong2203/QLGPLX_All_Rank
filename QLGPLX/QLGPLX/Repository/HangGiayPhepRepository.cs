using Microsoft.EntityFrameworkCore;
using QLGPLX.Data;
using QLGPLX.Models;

namespace QLGPLX.Repository;

public class HangGiayPhepRepository
{
    private readonly GplxDbContext _context;

    public HangGiayPhepRepository(GplxDbContext context)
    {
        _context = context;
    }

    public async Task<List<Hanggiayphep>> GetAllAsync()
    {
        return await _context.Hanggiaypheps
            .OrderBy(h => h.MaHang)
            .ToListAsync();
    }

    public async Task<Hanggiayphep?> GetByIdAsync(string maHang)
    {
        return await _context.Hanggiaypheps
            .FirstOrDefaultAsync(h => h.MaHang == maHang);
    }

    public async Task<bool> ExistsAsync(string maHang)
    {
        return await _context.Hanggiaypheps.AnyAsync(h => h.MaHang == maHang);
    }
}