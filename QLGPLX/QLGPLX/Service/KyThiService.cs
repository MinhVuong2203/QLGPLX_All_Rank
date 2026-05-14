using AutoMapper;
using Backend.DTO.KyThi;
using Backend.Service.Interface;
using Backend.Models;
using Backend.Repository;

namespace Backend.Service
{
    public class KyThiService : IKyThiService
    {
        private readonly KyThiRepository _repository;
        private readonly IMapper _mapper;

        public KyThiService(KyThiRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<KyThiDTO>> GetAllKyThiAsync()
        {
            var kyThis = await _repository.GetAllAsync();
            var result = new List<KyThiDTO>();

            foreach (var kt in kyThis)
            {
                var dto = _mapper.Map<KyThiDTO>(kt);
                dto.TenHang = kt.MaHangNavigation?.TenHang;
                result.Add(dto);
            }

            return result;
        }

        public async Task<KyThiDTO> GetKyThiByIdAsync(int id)
        {
            var kyThi = await _repository.GetByIdAsync(id);
            if (kyThi == null) return null;
            var dto = _mapper.Map<KyThiDTO>(kyThi);
            dto.TenHang = kyThi.MaHangNavigation?.TenHang;
            return dto;
        }

        public async Task<KyThiDTO> GetKyThiByPublicIdAsync(Guid publicId)
        {
            var kyThi = await _repository.GetByPublicIdAsync(publicId);
            if (kyThi == null) return null;

            var dto = _mapper.Map<KyThiDTO>(kyThi);
            dto.TenHang = kyThi.MaHangNavigation?.TenHang;
            return dto;
        }

        public async Task<KyThiDTO> CreateKyThiAsync(CreateKyThiDTO createDto)
        {
            var kyThi = _mapper.Map<Kythi>(createDto);
            var created = await _repository.CreateAsync(kyThi);

            var dto = _mapper.Map<KyThiDTO>(created);
            dto.TenHang = created.MaHangNavigation?.TenHang;
            dto.SoLuongDangKy = 0;
            return dto;
        }

        public async Task<KyThiDTO> UpdateKyThiAsync(int id, UpdateKyThiDTO updateDto)
        {
            var kyThi = await _repository.GetByIdAsync(id);
            if (kyThi == null) return null;

            _mapper.Map(updateDto, kyThi);
            var updated = await _repository.UpdateAsync(kyThi);

            var dto = _mapper.Map<KyThiDTO>(updated);
            dto.TenHang = updated.MaHangNavigation?.TenHang;  
            return dto;
        }

        public async Task<bool> DeleteKyThiAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<HoSoDaDuyetDTO>> GetHoSoDaDuyetAsync(string maHang)
        {
            var hoSos = await _repository.GetHoSoDaDuyetAsync(maHang);
            var result = new List<HoSoDaDuyetDTO>();

            foreach (var hs in hoSos)
            {
                var dto = new HoSoDaDuyetDTO
                {
                    HoSoID = hs.HoSoId,
                    PublicId = (Guid)hs.PublicId,
                    MaCongDan = hs.MaCongDan,
                    HoTenCongDan = hs.MaCongDanNavigation?.HoTen,
                    CCCD = hs.MaCongDanNavigation?.Cccd,
                    MaHang = hs.MaHang,
                    TenHang = hs.MaHangNavigation?.TenHang,
                    NgayNop = hs.NgayNop ?? DateTime.Now,                  
                    DaDangKyKyThi = false // Sẽ được cập nhật bên controller
                };
                result.Add(dto);
            }

            return result;
        }

        public async Task<List<HoSoDaDuyetDTO>> GetHoSoTrongKyThiAsync(int kyThiId)
        {
            var hoSos = await _repository.GetHoSoTrongKyThiAsync(kyThiId);
            var result = new List<HoSoDaDuyetDTO>();

            foreach (var hs in hoSos)
            {
                var dto = new HoSoDaDuyetDTO
                {
                    HoSoID = hs.HoSoId,
                    PublicId = (Guid)hs.PublicId,
                    MaCongDan = hs.MaCongDan,
                    HoTenCongDan = hs.MaCongDanNavigation?.HoTen,
                    CCCD = hs.MaCongDanNavigation?.Cccd,
                    MaHang = hs.MaHang,
                    TenHang = hs.MaHangNavigation?.TenHang,
                    NgayNop = hs.NgayNop ?? DateTime.Now,                   
                    DaDangKyKyThi = true
                };
                result.Add(dto);
            }

            return result;
        }

        public async Task<bool> ThemHoSoVaoKyThiAsync(ThemHoSoVaoKyThiDTO dto)
        {
            return await _repository.ThemHoSoVaoKyThiAsync(dto.KyThiID, dto.DanhSachHoSoID);
        }

        public async Task<bool> XoaHoSoKhoiKyThiAsync(int kyThiId, int hoSoId)
        {
            return await _repository.XoaHoSoKhoiKyThiAsync(kyThiId, hoSoId);
        }
    }
}
