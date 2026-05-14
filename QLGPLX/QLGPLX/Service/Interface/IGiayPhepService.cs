using Backend.DTO.GiayPhep;
using Backend.Models;

namespace Backend.Service.Interface
{
    public interface IGiayPhepService
    {
        Task<PagedResult<GiayPhepDTO>> GetGiayPhepsAsync(GiayPhepSearchDTO search);
        Task<GiayPhepDTO?> GetGiayPhepByIdAsync(int id);
        Task<Giayphep> CreateGiayPhepAsync(GiayPhepCreateDTO dto);
        Task<bool> UpdateGiayPhepAsync(int id, GiayPhepUpdateDTO dto);
        Task<object> GetStatisticsAsync();
    }
}
