using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class AuthRepository
    {
        private readonly GplxDbContext _context;

        public AuthRepository(GplxDbContext context)
        {
            _context = context;
        }

        public async Task<Canbo?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            usernameOrEmail = usernameOrEmail.Trim();

            return await _context.Canbos
                .Include(cb => cb.MaChucVuNavigation)
                .Include(cb => cb.MaChucNangs)
                .FirstOrDefaultAsync(cb =>
                    cb.Username == usernameOrEmail ||
                    cb.Email == usernameOrEmail
                );
        }

        public async Task<Canbo?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Canbos
                .Include(cb => cb.MaChucVuNavigation)
                .Include(cb => cb.MaChucNangs)
                .FirstOrDefaultAsync(cb => cb.PublicId == publicId);
        }
    }
}
