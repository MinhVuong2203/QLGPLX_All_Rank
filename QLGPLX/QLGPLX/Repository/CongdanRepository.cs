using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Repository
{
    public class CongdanRepository
    {
        private readonly GplxDbContext _context;

        public CongdanRepository(GplxDbContext context)
        {
            _context = context;
        }

        public List<Congdan> GetAll() => _context.Congdans.ToList();

        public Congdan? GetById(Guid id) => _context.Congdans.FirstOrDefault(t => t.PublicId == id);

        public async Task<Congdan?> GetByIdAsync(int id)
        {
            return await _context.Congdans
                .FirstOrDefaultAsync(t => t.MaCongDan == id);
        }

        public void Add(Congdan congdan)
        {
            _context.Congdans.Add(congdan);
            _context.SaveChanges();
        }

        public void Update(Congdan congdan)
        {
            _context.Congdans.Update(congdan);
            _context.SaveChanges();
        }

        public void Delete(Congdan congdan)
        {
            _context.Congdans.Remove(congdan);
            _context.SaveChanges();
        }

        public async Task<List<Congdan>> GetCongDanChuaCoHoSo()
        {
            return await _context.Congdans
                .Where(cd => !_context.Hosos.Any(h => h.MaCongDan == cd.MaCongDan))
                .ToListAsync();
        }

        public async Task<List<Congdan>> GetCongDanHomNay()
        {
            var today = DateTime.Today;

            return await _context.Congdans
                .Where(cd => cd.NgayTao.HasValue && cd.NgayTao.Value.Date == today)
                .ToListAsync();
        }

        public async Task<List<Congdan>> SearchByCCCD(string cccd)
        {
            return await _context.Congdans
                .Where(cd => cd.Cccd.Contains(cccd))
                .ToListAsync();
        }

    }
}
