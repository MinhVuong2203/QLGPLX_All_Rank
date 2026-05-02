
using Backend.DTO.HangGiayPhep;

namespace Backend.Service.Interface;
public interface IHangGiayPhepService
{
    Task<List<HangGiayPhepDTO>> GetAllAsync();
    Task<HangGiayPhepDTO?> GetByIdAsync(string maHang);
}