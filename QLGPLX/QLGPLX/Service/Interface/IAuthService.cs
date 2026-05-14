using Backend.DTO.Auth;

namespace Backend.Service.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);

        Task<AuthCanBoDto?> GetMeAsync(Guid publicId);
    }
}
