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
        Task<List<LichSuGiayPhepDTO>> GetLichSuAsync(int giayPhepId);
        Task<bool> DuyetGiayPhepAsync(int id, GiayPhepActionDTO dto);
        Task<bool> CapLaiGiayPhepAsync(int id, GiayPhepActionDTO dto);
        Task<object> GetStatisticsAsync();
    }
}
