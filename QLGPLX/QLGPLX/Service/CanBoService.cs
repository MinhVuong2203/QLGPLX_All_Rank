using Backend.DTO.CanBo;
using Backend.Models;
using Backend.Repository;
using Backend.Service.Interface;

namespace Backend.Service
{
    public class CanBoService : ICanBoService
    {   
        private readonly CanBoRepository _canBoRepository;
        private readonly ChucNangRepository _chucNangRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public CanBoService(CanBoRepository canBoRepository, ICloudinaryService cloudinaryService, ChucNangRepository chucNangRepository)
        {
            _canBoRepository = canBoRepository;
            _cloudinaryService = cloudinaryService;
            _chucNangRepository = chucNangRepository;
        }

        public async Task<List<CanBoResponseDto>> GetAllAsync(string? keyword, bool? trangThai)
        {
            return await _canBoRepository.GetAllAsync(keyword, trangThai);
        }

        public async Task<CanBoResponseDto?> GetByPublicIdAsync(Guid publicId)
        {
            return await _canBoRepository.GetByPublicIdAsync(publicId);
        }

        public async Task<bool> CreateAsync(CanBoCreateDto dto, IFormFile? anh3x4)
        {
            if (await _canBoRepository.ExistsEmailAsync(dto.Email.Trim()))
            {
                throw new Exception("Email đã tồn tại");
            }

            if (await _canBoRepository.ExistsCccdAsync(dto.Cccd.Trim()))
            {
                throw new Exception("CCCD đã tồn tại");
            }

            if (await _canBoRepository.ExistsUsernameAsync(dto.Username.Trim()))
            {
                throw new Exception("Username đã tồn tại");
            }

            var publicId = Guid.NewGuid();

            var canBo = new Canbo
            {
                PublicId = publicId,
                HoTen = dto.HoTen.Trim(),
                MaChucVu = dto.MaChucVu,
                Email = dto.Email.Trim(),
                Cccd = dto.Cccd.Trim(),
                DienThoai = dto.DienThoai?.Trim(),
                Username = dto.Username.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                NgayTao = DateTime.Now,
                TrangThai = true
            };

            // DB chỉ lưu path/url ảnh
            if (anh3x4 != null)
            {
                canBo.Anh3x4 = await _cloudinaryService.UploadImageAsync(
                    anh3x4,
                    "QLGPLX/canbo/avatar",
                    publicId.ToString()
                );
            }

            await _canBoRepository.AddAsync(canBo);

            return true;
        }

        public async Task<bool> UpdateAsync(Guid publicId, CanBoUpdateDto dto, IFormFile? anh3x4)
        {
            var canBo = await _canBoRepository.GetEntityByPublicIdAsync(publicId);

            if (canBo == null)
            {
                return false;
            }

            if (await _canBoRepository.ExistsEmailAsync(dto.Email.Trim(), publicId))
            {
                throw new Exception("Email đã tồn tại");
            }

            if (await _canBoRepository.ExistsCccdAsync(dto.Cccd.Trim(), publicId))
            {
                throw new Exception("CCCD đã tồn tại");
            }

            if (await _canBoRepository.ExistsUsernameAsync(dto.Username.Trim(), publicId))
            {
                throw new Exception("Username đã tồn tại");
            }

            canBo.HoTen = dto.HoTen.Trim();
            canBo.MaChucVu = dto.MaChucVu;
            canBo.Email = dto.Email.Trim();
            canBo.Cccd = dto.Cccd.Trim();
            canBo.DienThoai = dto.DienThoai?.Trim();
            canBo.Username = dto.Username.Trim();
            canBo.TrangThai = dto.TrangThai;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                canBo.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            // Có ảnh mới thì upload và thay path cũ
            // Không gửi ảnh thì giữ nguyên canBo.Anh3x4 hiện tại
            if (anh3x4 != null)
            {
                canBo.Anh3x4 = await _cloudinaryService.UploadImageAsync(
                    anh3x4,
                    "QLGPLX/canbo/avatar",
                    publicId.ToString()
                );
            }

            await _canBoRepository.UpdateAsync(canBo);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid publicId)
        {
            var canBo = await _canBoRepository.GetEntityByPublicIdAsync(publicId);

            if (canBo == null)
            {
                return false;
            }

            canBo.TrangThai = false;

            await _canBoRepository.UpdateAsync(canBo);

            return true;
        }

        public async Task<bool> ChangeStatusAsync(Guid publicId, bool trangThai)
        {
            var canBo = await _canBoRepository.GetEntityByPublicIdAsync(publicId);

            if (canBo == null)
            {
                return false;
            }

            await _canBoRepository.ChangeStatusAsync(canBo, trangThai);

            return true;
        }

        public async Task<List<Chucvu>> GetChucVuAsync()
        {
            return await _canBoRepository.GetChucVuAsync();
        }

        public async Task<List<int>> GetQuyenByCanBoAsync(Guid publicId)
        {
            return await _canBoRepository.GetQuyenByCanBoAsync(publicId);
        }

        public async Task<bool> UpdateQuyenAsync(Guid publicId, PhanQuyenCanBoDto dto)
        {
            var canBo = await _canBoRepository.GetEntityByPublicIdAsync(publicId);

            if (canBo == null)
            {
                return false;
            }

            var maChucNangs = dto.MaChucNangs
                .Distinct()
                .ToList();

            var chucNangs = await _chucNangRepository.GetAllAsync();

            var quyenQuanLyCanBo = chucNangs.FirstOrDefault(x =>
                x.MaChucNangCode == "QUAN_LY_CAN_BO"
            );

            if (quyenQuanLyCanBo != null && maChucNangs.Contains(quyenQuanLyCanBo.MaChucNang))
            {
                throw new Exception("Không được cấp quyền Quản lý cán bộ tại màn hình phân quyền");
            }

            await _canBoRepository.UpdateQuyenAsync(publicId, maChucNangs);

            return true;
        }
    }
}

