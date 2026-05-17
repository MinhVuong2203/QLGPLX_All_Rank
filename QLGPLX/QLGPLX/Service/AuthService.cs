using Backend.DTO.Auth;
using Backend.Models;
using Backend.Repository;
using Backend.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Service
{
    public class AuthService : IAuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            AuthRepository authRepository,
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<AuthService> logger
        )
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var canBo = await _authRepository.GetByUsernameOrEmailAsync(dto.UsernameOrEmail);

            if (canBo == null)
            {
                throw new Exception("Tài khoản hoặc mật khẩu không đúng");
            }

            if (canBo.TrangThai != true)
            {
                throw new Exception("Tài khoản đã bị khóa hoặc ngưng hoạt động");
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, canBo.PasswordHash);

            if (!isValidPassword)
            {
                throw new Exception("Tài khoản hoặc mật khẩu không đúng");
            }

            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "480");
            var expiredAt = DateTime.Now.AddMinutes(expireMinutes);

            var quyen = canBo.MaChucNangs
                .Where(cn => cn.TrangThai == true)
                .Select(cn => cn.MaChucNangCode)
                .ToList();

            var token = GenerateJwtToken(canBo.PublicId, canBo.Username, canBo.Email, quyen, expiredAt);

            return new LoginResponseDto
            {
                Token = token,
                ExpiredAt = expiredAt,
                CanBo = new AuthCanBoDto
                {
                    PublicId = canBo.PublicId,
                    HoTen = canBo.HoTen,
                    Email = canBo.Email,
                    Username = canBo.Username,
                    Anh3x4 = canBo.Anh3x4,
                    MaChucVu = canBo.MaChucVu,
                    TenChucVu = canBo.MaChucVuNavigation?.TenChucVu,
                    Quyen = quyen
                }
            };
        }

        public async Task<AuthCanBoDto?> GetMeAsync(Guid publicId)
        {
            var canBo = await _authRepository.GetByPublicIdAsync(publicId);

            if (canBo == null)
            {
                return null;
            }

            var quyen = canBo.MaChucNangs
                .Where(cn => cn.TrangThai == true)
                .Select(cn => cn.MaChucNangCode)
                .ToList();

            return new AuthCanBoDto
            {
                PublicId = canBo.PublicId,
                HoTen = canBo.HoTen,
                Email = canBo.Email,
                Username = canBo.Username,
                Anh3x4 = canBo.Anh3x4,
                MaChucVu = canBo.MaChucVu,
                TenChucVu = canBo.MaChucVuNavigation?.TenChucVu,
                Quyen = quyen
            };
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var canBo = await _authRepository.GetByEmailAsync(dto.Email);

            if (canBo == null || canBo.TrangThai != true)
            {
                return;
            }

            var otpCode = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            var expiredAt = DateTime.Now.AddMinutes(10);

            await _authRepository.InvalidatePasswordResetOtpsAsync(canBo.MaCanBo);
            await _authRepository.CreatePasswordResetOtpAsync(new Passwordresetotp
            {
                MaCanBo = canBo.MaCanBo,
                Otpcode = otpCode,
                ExpiredAt = expiredAt,
                IsUsed = false,
                NgayTao = DateTime.Now
            });
            await _authRepository.SaveChangesAsync();

            _ = TrySendPasswordResetOtpAsync(canBo, otpCode, expiredAt);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var canBo = await _authRepository.GetByEmailAsync(dto.Email);
            if (canBo == null || canBo.TrangThai != true)
            {
                throw new Exception("Email hoặc mã OTP không hợp lệ");
            }

            var otp = await _authRepository.GetValidPasswordResetOtpAsync(canBo.MaCanBo, dto.OtpCode);
            if (otp == null)
            {
                throw new Exception("Email hoặc mã OTP không hợp lệ hoặc đã hết hạn");
            }

            canBo.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            otp.IsUsed = true;

            await _authRepository.SaveChangesAsync();
        }

        private async Task TrySendPasswordResetOtpAsync(Canbo canBo, string otpCode, DateTime expiredAt)
        {
            try
            {
                await _emailService.SendPasswordResetOtpAsync(canBo, otpCode, expiredAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Khong gui duoc OTP dat lai mat khau den {Email}", canBo.Email);
            }
        }

        private string GenerateJwtToken(
            Guid publicId,
            string username,
            string email,
            List<string> quyen,
            DateTime expiredAt
        )
        {
            var key = _configuration["Jwt:Key"] ?? throw new Exception("JWT Key chưa được cấu hình");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, publicId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim("publicId", publicId.ToString()),
                new Claim("username", username)
            };

            foreach (var item in quyen)
            {
                claims.Add(new Claim("permission", item));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiredAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
