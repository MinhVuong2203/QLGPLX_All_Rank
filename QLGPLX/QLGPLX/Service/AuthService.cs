using Backend.DTO.Auth;
using Backend.Repository;
using Backend.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Service
{
    public class AuthService : IAuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            AuthRepository authRepository,
            IConfiguration configuration
        )
        {
            _authRepository = authRepository;
            _configuration = configuration;
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
