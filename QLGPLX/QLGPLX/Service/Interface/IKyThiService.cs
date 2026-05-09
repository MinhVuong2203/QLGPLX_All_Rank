using Backend.DTO.KyThi;

namespace Backend.Service.Interface
{
    public interface IKyThiService
    {
        Task<List<KyThiDTO>> GetAllKyThiAsync();
        Task<KyThiDTO> GetKyThiByIdAsync(int id);
        Task<KyThiDTO> GetKyThiByPublicIdAsync(Guid publicId);
        Task<KyThiDTO> CreateKyThiAsync(CreateKyThiDTO createDto);
        Task<KyThiDTO> UpdateKyThiAsync(int id, UpdateKyThiDTO updateDto);
        Task<bool> DeleteKyThiAsync(int id);
        Task<List<HoSoDaDuyetDTO>> GetHoSoDaDuyetAsync(string maHang);
        Task<List<HoSoDaDuyetDTO>> GetHoSoTrongKyThiAsync(int kyThiId);
        Task<bool> ThemHoSoVaoKyThiAsync(ThemHoSoVaoKyThiDTO dto);
        Task<bool> XoaHoSoKhoiKyThiAsync(int kyThiId, int hoSoId);
    }
}
