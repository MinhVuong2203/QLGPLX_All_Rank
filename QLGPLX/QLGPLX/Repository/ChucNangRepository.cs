using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class ChucNangRepository
    {
        private readonly GplxDbContext _context;

        public ChucNangRepository(GplxDbContext context)
        {
            _context = context;
        }

        public async Task<List<Chucnang>> GetAllAsync()
        {
            return await _context.Chucnangs
                .Where(cn => cn.TrangThai == true)
                .OrderBy(cn => cn.MaChucNang)
                .ToListAsync();
        }

        public async Task<Chucnang?> GetByIdAsync(int id)
        {
            return await _context.Chucnangs
                .FirstOrDefaultAsync(cn => cn.MaChucNang == id);
        }
    }
}
