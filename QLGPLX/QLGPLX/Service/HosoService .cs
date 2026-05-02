using AutoMapper;
using Backend.DTO.HoSo;
using Backend.Service.Interface;
using QLGPLX.Models;
using QLGPLX.Repository;


namespace Backend.Service;

public class HosoService : IHosoService
{
    private readonly HosoRepository _hosoRepository;
    private readonly CongdanRepository _congdanRepository;
    private readonly HangGiayPhepRepository _hangRepository;
    private readonly IMapper _mapper;

    public HosoService(
        HosoRepository hosoRepository,
        CongdanRepository congdanRepository,
        HangGiayPhepRepository hangRepository,
        IMapper mapper)
    {
        _hosoRepository = hosoRepository;
        _congdanRepository = congdanRepository;
        _hangRepository = hangRepository;
        _mapper = mapper;
    }

    public async Task<List<HosoDTO>> GetAllAsync()
    {
        var hosos = await _hosoRepository.GetAllAsync();
        return hosos.Select(h => new HosoDTO
        {
            HoSoId = h.HoSoId,
            PublicId = h.PublicId,
            MaCongDan = h.MaCongDan,
            MaHang = h.MaHang,
            NgayNop = h.NgayNop,
            TrangThai = h.TrangThai,
            TrangThaiThanhToan = h.TrangThaiThanhToan,
            GhiChu = h.GhiChu,
            TenCongDan = h.MaCongDanNavigation?.HoTen,
            CCCD = h.MaCongDanNavigation?.Cccd,
            TenHang = h.MaHangNavigation?.TenHang
        }).ToList();
    }

    public async Task<HosoDTO?> GetByIdAsync(int id)
    {
        var hoso = await _hosoRepository.GetByIdAsync(id);
        if (hoso == null) return null;

        return new HosoDTO
        {
            HoSoId = hoso.HoSoId,
            PublicId = hoso.PublicId,
            MaCongDan = hoso.MaCongDan,
            MaHang = hoso.MaHang,
            NgayNop = hoso.NgayNop,
            TrangThai = hoso.TrangThai,
            TrangThaiThanhToan = hoso.TrangThaiThanhToan,
            GhiChu = hoso.GhiChu,
            TenCongDan = hoso.MaCongDanNavigation?.HoTen,
            CCCD = hoso.MaCongDanNavigation?.Cccd,
            TenHang = hoso.MaHangNavigation?.TenHang
        };
    }

    public async Task<HosoDTO?> GetByPublicIdAsync(Guid publicId)
    {
        var hoso = await _hosoRepository.GetByPublicIdAsync(publicId);
        if (hoso == null) return null;

        return new HosoDTO
        {
            HoSoId = hoso.HoSoId,
            PublicId = hoso.PublicId,
            MaCongDan = hoso.MaCongDan,
            MaHang = hoso.MaHang,
            NgayNop = hoso.NgayNop,
            TrangThai = hoso.TrangThai,
            TrangThaiThanhToan = hoso.TrangThaiThanhToan,
            GhiChu = hoso.GhiChu,
            TenCongDan = hoso.MaCongDanNavigation?.HoTen,
            CCCD = hoso.MaCongDanNavigation?.Cccd,
            TenHang = hoso.MaHangNavigation?.TenHang
        };
    }

    public async Task<HosoDTO> CreateAsync(CreateHosoDTO dto)
    {
        // Kiểm tra công dân tồn tại
        var congdan = await _congdanRepository.GetByIdAsync(dto.MaCongDan);
        if (congdan == null)
            throw new ArgumentException("Công dân không tồn tại");

        // Kiểm tra hạng GPLX tồn tại
        var hang = await _hangRepository.GetByIdAsync(dto.MaHang);
        if (hang == null)
            throw new ArgumentException("Hạng GPLX không tồn tại");

        var hoso = new Hoso
        {
            PublicId = Guid.NewGuid(),
            MaCongDan = dto.MaCongDan,
            MaHang = dto.MaHang,
            NgayNop = DateTime.Now,
            TrangThai = "Đang xử lý",
            TrangThaiThanhToan = false,
            GhiChu = dto.GhiChu
        };

        var created = await _hosoRepository.CreateAsync(hoso);
        
        // Load navigation properties
        var result = await _hosoRepository.GetByIdAsync(created.HoSoId);
        
        return new HosoDTO
        {
            HoSoId = result!.HoSoId,
            PublicId = result.PublicId,
            MaCongDan = result.MaCongDan,
            MaHang = result.MaHang,
            NgayNop = result.NgayNop,
            TrangThai = result.TrangThai,
            TrangThaiThanhToan = result.TrangThaiThanhToan,
            GhiChu = result.GhiChu,
            TenCongDan = result.MaCongDanNavigation?.HoTen,
            CCCD = result.MaCongDanNavigation?.Cccd,
            TenHang = result.MaHangNavigation?.TenHang
        };
    }

    public async Task<HosoDTO?> UpdateAsync(int id, CreateHosoDTO dto)
    {
        var hoso = await _hosoRepository.GetByIdAsync(id);
        if (hoso == null) return null;

        // Kiểm tra công dân tồn tại
        var congdan = await _congdanRepository.GetByIdAsync(dto.MaCongDan);
        if (congdan == null)
            throw new ArgumentException("Công dân không tồn tại");

        // Kiểm tra hạng GPLX tồn tại
        var hang = await _hangRepository.GetByIdAsync(dto.MaHang);
        if (hang == null)
            throw new ArgumentException("Hạng GPLX không tồn tại");

        hoso.MaCongDan = dto.MaCongDan;
        hoso.MaHang = dto.MaHang;
        hoso.GhiChu = dto.GhiChu;

        await _hosoRepository.UpdateAsync(hoso);
        
        var result = await _hosoRepository.GetByIdAsync(id);
        
        return new HosoDTO
        {
            HoSoId = result!.HoSoId,
            PublicId = result.PublicId,
            MaCongDan = result.MaCongDan,
            MaHang = result.MaHang,
            NgayNop = result.NgayNop,
            TrangThai = result.TrangThai,
            TrangThaiThanhToan = result.TrangThaiThanhToan,
            GhiChu = result.GhiChu,
            TenCongDan = result.MaCongDanNavigation?.HoTen,
            CCCD = result.MaCongDanNavigation?.Cccd,
            TenHang = result.MaHangNavigation?.TenHang
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _hosoRepository.DeleteAsync(id);
    }

    public async Task<List<HosoDTO>> GetByCongDanAsync(int maCongDan)
    {
        var hosos = await _hosoRepository.GetByCongDanAsync(maCongDan);
        return hosos.Select(h => new HosoDTO
        {
            HoSoId = h.HoSoId,
            PublicId = h.PublicId,
            MaCongDan = h.MaCongDan,
            MaHang = h.MaHang,
            NgayNop = h.NgayNop,
            TrangThai = h.TrangThai,
            TrangThaiThanhToan = h.TrangThaiThanhToan,
            GhiChu = h.GhiChu,
            TenCongDan = h.MaCongDanNavigation?.HoTen,
            CCCD = h.MaCongDanNavigation?.Cccd,
            TenHang = h.MaHangNavigation?.TenHang
        }).ToList();
    }
}