using Microsoft.EntityFrameworkCore;
using QLGPLX.Data;
using QLGPLX.Models;

namespace QLGPLX.Repository
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
        
    }
}
