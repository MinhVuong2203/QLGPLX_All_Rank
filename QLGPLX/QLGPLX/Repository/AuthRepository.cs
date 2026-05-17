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

        public async Task<Canbo?> GetByEmailAsync(string email)
        {
            email = email.Trim();

            return await _context.Canbos
                .FirstOrDefaultAsync(cb => cb.Email == email);
        }

        public async Task InvalidatePasswordResetOtpsAsync(int maCanBo)
        {
            var otps = await _context.Passwordresetotps
                .Where(otp => otp.MaCanBo == maCanBo && otp.IsUsed != true)
                .ToListAsync();

            foreach (var otp in otps)
            {
                otp.IsUsed = true;
            }
        }

        public async Task CreatePasswordResetOtpAsync(Passwordresetotp otp)
        {
            await _context.Passwordresetotps.AddAsync(otp);
        }

        public async Task<Passwordresetotp?> GetValidPasswordResetOtpAsync(int maCanBo, string otpCode)
        {
            return await _context.Passwordresetotps
                .Where(otp =>
                    otp.MaCanBo == maCanBo &&
                    otp.Otpcode == otpCode &&
                    otp.IsUsed != true &&
                    otp.ExpiredAt >= DateTime.Now)
                .OrderByDescending(otp => otp.NgayTao)
                .FirstOrDefaultAsync();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
