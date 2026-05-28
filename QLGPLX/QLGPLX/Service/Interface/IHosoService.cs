
using Backend.DTO.HoSo;

namespace Backend.Service.Interface;
public interface IHosoService
{
    Task<List<HosoDTO>> GetAllAsync();
    Task<HosoDTO?> GetByIdAsync(int id);
    Task<HosoDTO?> GetByPublicIdAsync(Guid publicId);
    Task<HosoDTO> CreateAsync(CreateHosoDTO dto);
    Task<HosoDTO?> UpdateAsync(int id, UpdateHosoDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<List<HosoDTO>> GetByCongDanAsync(int maCongDan);
    Task<bool> ExistsByMaCongDanAndMaHangAsync(int maCongDan, string maHang);
    Task<HoSoDieuKienDangKyDTO> KiemTraDieuKienDangKyAsync(int? maCongDan, string maHang);
}
