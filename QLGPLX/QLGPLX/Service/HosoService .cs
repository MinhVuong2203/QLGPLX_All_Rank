using AutoMapper;
using Backend.DTO.HoSo;
using Backend.Repository;
using Backend.Service.Interface;
using Backend.Models;

namespace Backend.Service;

public class HosoService : IHosoService
{
    private readonly HosoRepository _hosoRepository;
    private readonly CongdanRepository _congdanRepository;
    private readonly HangGiayPhepRepository _hangRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<HosoService> _logger;
    private readonly IMapper _mapper;

    public HosoService(
        HosoRepository hosoRepository,
        CongdanRepository congdanRepository,
        HangGiayPhepRepository hangRepository,
        IEmailService emailService,
        ILogger<HosoService> logger,
        IMapper mapper)
    {
        _hosoRepository = hosoRepository;
        _congdanRepository = congdanRepository;
        _hangRepository = hangRepository;
        _emailService = emailService;
        _logger = logger;
        _mapper = mapper;
    }

    // ================= GET =================
    public async Task<List<HosoDTO>> GetAllAsync()
    {
        var list = await _hosoRepository.GetAllAsync();
        return _mapper.Map<List<HosoDTO>>(list);
    }

    public async Task<HosoDTO?> GetByIdAsync(int id)
    {
        var hoso = await _hosoRepository.GetByIdAsync(id);
        return hoso == null ? null : _mapper.Map<HosoDTO>(hoso);
    }

    public async Task<HosoDTO?> GetByPublicIdAsync(Guid publicId)
    {
        var hoso = await _hosoRepository.GetByPublicIdAsync(publicId);
        return hoso == null ? null : _mapper.Map<HosoDTO>(hoso);
    }

    public async Task<List<HosoDTO>> GetByCongDanAsync(int maCongDan)
    {
        var list = await _hosoRepository.GetByCongDanAsync(maCongDan);
        return _mapper.Map<List<HosoDTO>>(list);
    }

    // ================= CREATE =================
    public async Task<HosoDTO> CreateAsync(CreateHosoDTO dto)
    {
        var congdan = await _congdanRepository.GetByIdAsync(dto.MaCongDan);
        if (congdan == null)
            throw new ArgumentException("Công dân không tồn tại");

        var hang = await _hangRepository.GetByIdAsync(dto.MaHang);
        if (hang == null)
            throw new ArgumentException("Hạng GPLX không tồn tại");

        var exists = await _hosoRepository
            .ExistsByMaCongDanAndMaHangAsync(dto.MaCongDan, dto.MaHang);

        if (exists)
            throw new InvalidOperationException($"Đã tồn tại hồ sơ hạng {dto.MaHang}");

        var hoso = _mapper.Map<Hoso>(dto);

        hoso.PublicId = Guid.NewGuid();
        hoso.NgayNop = DateTime.Now;
       
        var created = await _hosoRepository.CreateAsync(hoso);
        var result = await _hosoRepository.GetByIdAsync(created.HoSoId);

        _ = TrySendEmailAsync(
            () => _emailService.SendHoSoCreatedAsync(congdan, result ?? created),
            congdan.Email);

        return _mapper.Map<HosoDTO>(result);
    }

    // ================= UPDATE (DÙNG UpdateHosoDTO) =================
    public async Task<HosoDTO?> UpdateAsync(int id, UpdateHosoDTO dto)
    {
        var hoso = await _hosoRepository.GetByIdAsync(id);
        if (hoso == null) return null;

        // ❗ Không cho đổi công dân
        // giữ nguyên hoso.MaCongDan

        var hang = await _hangRepository.GetByIdAsync(dto.MaHang);
        if (hang == null)
            throw new ArgumentException("Hạng GPLX không tồn tại");

        // check trùng (trừ chính nó)
        var exists = await _hosoRepository
            .ExistsByMaCongDanAndMaHangAsync(hoso.MaCongDan, dto.MaHang);

        if (exists && hoso.MaHang != dto.MaHang)
            throw new InvalidOperationException("Đã tồn tại hồ sơ");

        // 🔥 map tự động
        _mapper.Map(dto, hoso);

        await _hosoRepository.UpdateAsync(hoso);

        var result = await _hosoRepository.GetByIdAsync(id);
        return _mapper.Map<HosoDTO>(result);
    }

    // ================= DELETE =================
    public async Task<bool> DeleteAsync(int id)
    {
        return await _hosoRepository.DeleteAsync(id);
    }

    // ================= CHECK =================
    public async Task<bool> ExistsByMaCongDanAndMaHangAsync(int maCongDan, string maHang)
    {
        return await _hosoRepository.ExistsByMaCongDanAndMaHangAsync(maCongDan, maHang);
    }

    private async Task TrySendEmailAsync(Func<Task> sendEmail, string? email)
    {
        try
        {
            await sendEmail();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Khong gui duoc email ho so den {Email}", email);
        }
    }
}
