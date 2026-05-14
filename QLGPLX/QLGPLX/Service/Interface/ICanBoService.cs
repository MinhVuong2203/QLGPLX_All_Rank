using Backend.DTO.CanBo;
using Backend.Models;

namespace Backend.Service.Interface
{
    public interface ICanBoService
    {
        Task<List<CanBoResponseDto>> GetAllAsync(string? keyword, bool? trangThai);

        Task<CanBoResponseDto?> GetByPublicIdAsync(Guid publicId);

        Task<bool> CreateAsync(CanBoCreateDto dto, IFormFile? anh3x4);
        Task<bool> UpdateAsync(Guid publicId, CanBoUpdateDto dto, IFormFile? anh3x4);

        Task<bool> DeleteAsync(Guid publicId);

        Task<bool> ChangeStatusAsync(Guid publicId, bool trangThai);

        Task<List<Chucvu>> GetChucVuAsync();

        Task<List<int>> GetQuyenByCanBoAsync(Guid publicId);

        Task<bool> UpdateQuyenAsync(Guid publicId, PhanQuyenCanBoDto dto);
    }
}
