using Backend.DTO.Auth;

namespace Backend.Service.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);

        Task<AuthCanBoDto?> GetMeAsync(Guid publicId);

        Task ForgotPasswordAsync(ForgotPasswordDto dto);

        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
