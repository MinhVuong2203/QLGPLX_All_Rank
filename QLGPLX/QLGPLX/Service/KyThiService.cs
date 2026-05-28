using AutoMapper;
using Backend.DTO.KyThi;
using Backend.Service.Interface;
using Backend.Models;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class KyThiService : IKyThiService
    {
        private readonly KyThiRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ILogger<KyThiService> _logger;
        private readonly IMapper _mapper;

        public KyThiService(
            KyThiRepository repository,
            IEmailService emailService,
            ILogger<KyThiService> logger,
            IMapper mapper)
        {
            _repository = repository;
            _emailService = emailService;
            _logger = logger;
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

            ApplyUpdateByStatus(kyThi, updateDto);

            var updated = await _repository.UpdateAsync(kyThi);

            var dto = _mapper.Map<KyThiDTO>(updated);
            dto.TenHang = updated.MaHangNavigation?.TenHang;  
            return dto;
        }

        public async Task<bool> DeleteKyThiAsync(int id)
        {
            var kyThi = await _repository.GetByIdAsync(id);
            if (kyThi == null) return false;

            if (GetTrangThai(kyThi) != "Sắp diễn ra")
                throw new InvalidOperationException("Chỉ được xóa kỳ thi sắp diễn ra");

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
            var kyThi = await _repository.GetByIdAsync(dto.KyThiID);
            if (kyThi == null)
                return false;

            if (GetTrangThai(kyThi) == "Đã kết thúc")
                throw new InvalidOperationException("Kỳ thi đã kết thúc, không thể thêm thí sinh");

            var success = await _repository.ThemHoSoVaoKyThiAsync(dto.KyThiID, dto.DanhSachHoSoID);
            if (!success)
                return false;

            var hoSos = await _repository.GetHoSoByIdsAsync(dto.DanhSachHoSoID);

            foreach (var hoSo in hoSos)
            {
                if (hoSo.MaCongDanNavigation == null)
                    continue;

                _ = TrySendEmailAsync(
                    () => _emailService.SendHoSoAddedToKyThiAsync(
                        hoSo.MaCongDanNavigation,
                        hoSo,
                        kyThi),
                    hoSo.MaCongDanNavigation?.Email);
            }

            return true;
        }

        public async Task<bool> XoaHoSoKhoiKyThiAsync(int kyThiId, int hoSoId)
        {
            return await _repository.XoaHoSoKhoiKyThiAsync(kyThiId, hoSoId);
        }

        private async Task TrySendEmailAsync(Func<Task> sendEmail, string? email)
        {
            try
            {
                await sendEmail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Khong gui duoc email ky thi den {Email}", email);
            }
        }

        private static void ApplyUpdateByStatus(Kythi kyThi, UpdateKyThiDTO updateDto)
        {
            var status = GetTrangThai(kyThi);
            var today = VietnamTime.TodayDate;

            if (status == "Đã kết thúc")
            {
                throw new InvalidOperationException("Kỳ thi đã kết thúc, không thể chỉnh sửa");
            }

            if (status == "Đang diễn ra")
            {
                if (!StringEquals(updateDto.TenKyThi, kyThi.TenKyThi))
                    throw new InvalidOperationException("Kỳ thi đang diễn ra không được đổi tên kỳ thi");

                if (updateDto.NgayBatDau != kyThi.NgayBatDau)
                    throw new InvalidOperationException("Kỳ thi đang diễn ra không được đổi ngày bắt đầu");

                if (!StringEquals(updateDto.MaHang, kyThi.MaHang))
                    throw new InvalidOperationException("Kỳ thi đang diễn ra không được đổi hạng GPLX");

                if (updateDto.NgayKetThuc < today)
                    throw new InvalidOperationException("Ngày kết thúc phải lớn hơn hoặc bằng ngày hiện tại");

                var soLuongDangKy = kyThi.SoLuongDangKy ?? 0;
                if (updateDto.SoLuongToiDa <= soLuongDangKy)
                    throw new InvalidOperationException("Số lượng tối đa phải lớn hơn số lượng đã đăng ký");

                kyThi.NgayKetThuc = updateDto.NgayKetThuc;
                kyThi.SoLuongToiDa = updateDto.SoLuongToiDa;
                kyThi.DiaDiem = updateDto.DiaDiem?.Trim();
                return;
            }

            ValidateUpcomingUpdate(updateDto, today);

            kyThi.TenKyThi = updateDto.TenKyThi?.Trim();
            kyThi.NgayBatDau = updateDto.NgayBatDau;
            kyThi.NgayKetThuc = updateDto.NgayKetThuc;
            kyThi.DiaDiem = updateDto.DiaDiem?.Trim();
            kyThi.MaHang = updateDto.MaHang?.Trim();
            kyThi.SoLuongToiDa = updateDto.SoLuongToiDa;
        }

        private static void ValidateUpcomingUpdate(UpdateKyThiDTO updateDto, DateOnly today)
        {
            if (string.IsNullOrWhiteSpace(updateDto.TenKyThi))
                throw new InvalidOperationException("Tên kỳ thi không được để trống");

            if (updateDto.TenKyThi.Length > 150)
                throw new InvalidOperationException("Tên kỳ thi tối đa 150 ký tự");

            if (string.IsNullOrWhiteSpace(updateDto.MaHang))
                throw new InvalidOperationException("Hạng GPLX không được để trống");

            if (updateDto.NgayBatDau < today)
                throw new InvalidOperationException("Ngày bắt đầu không hợp lệ");

            if (updateDto.NgayKetThuc < updateDto.NgayBatDau)
                throw new InvalidOperationException("Ngày kết thúc phải sau ngày bắt đầu");

            if (updateDto.SoLuongToiDa < 1)
                throw new InvalidOperationException("Số lượng phải lớn hơn 0");
        }

        private static string GetTrangThai(Kythi kyThi)
        {
            var today = VietnamTime.TodayDate;

            if (kyThi.NgayBatDau > today) return "Sắp diễn ra";
            if (kyThi.NgayKetThuc < today) return "Đã kết thúc";
            return "Đang diễn ra";
        }

        private static bool StringEquals(string? left, string? right)
        {
            return string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
