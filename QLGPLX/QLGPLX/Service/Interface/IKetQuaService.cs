using Backend.DTO.KetQua;

namespace Backend.Service.Interface
{
    public interface IKetQuaService
    {
        // Kết quả
        Task<List<HoSoKetQuaDTO>> GetHoSoKetQuaByKyThiAsync(int kyThiId);
        Task<KetQuaThiDTO> GetKetQuaByIdAsync(int ketQuaId);
        Task<KetQuaThiDTO> CreateKetQuaAsync(CreateKetQuaDTO dto);
        Task<KetQuaThiDTO> UpdateKetQuaAsync(int ketQuaId, UpdateKetQuaDTO dto);
        Task<bool> DeleteKetQuaAsync(int ketQuaId);

        // Kết quả chi tiết
        Task<KetQuaChiTietDTO> CreateKetQuaChiTietAsync(int ketQuaId, CreateKetQuaChiTietDTO dto);
        Task<KetQuaChiTietDTO> UpdateKetQuaChiTietAsync(int chiTietId, UpdateKetQuaChiTietDTO dto);
        Task<bool> DeleteKetQuaChiTietAsync(int chiTietId);
        Task<List<KetQuaChiTietDTO>> GetKetQuaChiTietByKetQuaIdAsync(int ketQuaId);
    }
}