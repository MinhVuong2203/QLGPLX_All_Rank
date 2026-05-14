namespace Backend.DTO.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiredAt { get; set; }

        public AuthCanBoDto CanBo { get; set; } = new();
    }
}
