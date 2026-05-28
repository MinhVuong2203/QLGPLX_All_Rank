using AutoMapper;
using Backend.Data;
using Backend.DTO.HoSo;
using Backend.Repository;
using Backend.Service.Interface;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service;

public class HosoService : IHosoService
{
    private const string TrangThaiChoDuyet = "Chờ duyệt";
    private const string TrangThaiBiThuHoi = "Bị thu hồi";
    private const string TrangThaiHetHan = "Hết hạn";

    private readonly GplxDbContext _context;
    private readonly HosoRepository _hosoRepository;
    private readonly CongdanRepository _congdanRepository;
    private readonly HangGiayPhepRepository _hangRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<HosoService> _logger;
    private readonly IMapper _mapper;

    public HosoService(
        GplxDbContext context,
        HosoRepository hosoRepository,
        CongdanRepository congdanRepository,
        HangGiayPhepRepository hangRepository,
        IEmailService emailService,
        ILogger<HosoService> logger,
        IMapper mapper)
    {
        _context = context;
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
            throw new ArgumentException($"Đã tồn tại hồ sơ hạng {dto.MaHang}");

        var dieuKienDangKy = await KiemTraDieuKienDangKyAsync(dto.MaCongDan, dto.MaHang);
        if (!dieuKienDangKy.DuDieuKien)
            throw new ArgumentException(dieuKienDangKy.ThongBao);

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
            throw new ArgumentException("Đã tồn tại hồ sơ");

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

    public async Task<HoSoDieuKienDangKyDTO> KiemTraDieuKienDangKyAsync(int? maCongDan, string maHang)
    {
        if (string.IsNullOrWhiteSpace(maHang))
            throw new ArgumentException("Vui lòng chọn hạng GPLX");

        var hangDangKy = await _context.Hanggiaypheps
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.MaHang == maHang);

        if (hangDangKy == null)
            throw new ArgumentException("Hạng GPLX không tồn tại");

        if (maCongDan.HasValue)
        {
            var congDanExists = await _context.Congdans
                .AsNoTracking()
                .AnyAsync(c => c.MaCongDan == maCongDan.Value);

            if (!congDanExists)
                throw new ArgumentException("Công dân không tồn tại");
        }

        var dieuKiens = await _context.DieuKienHangGplxes
            .AsNoTracking()
            .Include(d => d.HangBatBuoc)
            .Where(d => d.HangDangKyId == maHang)
            .OrderBy(d => d.HangBatBuocId)
            .ToListAsync();

        if (!dieuKiens.Any())
        {
            return new HoSoDieuKienDangKyDTO
            {
                MaCongDan = maCongDan,
                MaHangDangKy = maHang,
                CoYeuCauGiayPhepKhac = false,
                DuDieuKien = true,
                ThongBao = $"Hạng {maHang} không yêu cầu GPLX điều kiện, có thể tạo hồ sơ trực tiếp."
            };
        }

        var giayPheps = maCongDan.HasValue
            ? await _context.Giaypheps
                .AsNoTracking()
                .Where(g => g.MaCongDan == maCongDan.Value)
                .ToListAsync()
            : new List<Giayphep>();

        var today = DateOnly.FromDateTime(DateTime.Today);
        var ketQuaDieuKiens = dieuKiens
            .Select(d => KiemTraMotDieuKien(d, giayPheps, today, maCongDan.HasValue))
            .ToList();

        var duDieuKien = !maCongDan.HasValue || ketQuaDieuKiens.Any(d => d.DuDieuKien);
        var moTaDieuKien = string.Join(" hoặc ", dieuKiens.Select(d =>
            d.NamToiThieu > 0
                ? $"{d.HangBatBuocId} (tối thiểu {d.NamToiThieu} năm)"
                : d.HangBatBuocId));

        return new HoSoDieuKienDangKyDTO
        {
            MaCongDan = maCongDan,
            MaHangDangKy = maHang,
            CoYeuCauGiayPhepKhac = true,
            DuDieuKien = duDieuKien,
            DieuKiens = ketQuaDieuKiens,
            ThongBao = !maCongDan.HasValue
                ? $"Hạng {maHang} yêu cầu có GPLX {moTaDieuKien}. GPLX điều kiện phải không chờ duyệt, không bị thu hồi, chưa hết hạn và đủ số năm tối thiểu."
                : duDieuKien
                    ? $"Công dân đủ điều kiện đăng ký hạng {maHang}."
                    : $"Không thể tạo hồ sơ hạng {maHang}. Công dân cần có GPLX {moTaDieuKien}; GPLX phải không chờ duyệt, không bị thu hồi, chưa hết hạn và đủ số năm tối thiểu."
        };
    }

    private static HoSoDieuKienHangDTO KiemTraMotDieuKien(
        DieuKienHangGplx dieuKien,
        List<Giayphep> giayPheps,
        DateOnly today,
        bool coCongDan)
    {
        var result = new HoSoDieuKienHangDTO
        {
            HangBatBuocId = dieuKien.HangBatBuocId,
            TenHangBatBuoc = dieuKien.HangBatBuoc?.TenHang,
            NamToiThieu = dieuKien.NamToiThieu,
            DuDieuKien = !coCongDan,
            LyDo = coCongDan ? null : "Chưa chọn công dân để kiểm tra"
        };

        if (!coCongDan)
            return result;

        var cungHang = giayPheps
            .Where(g => g.MaHang == dieuKien.HangBatBuocId)
            .OrderByDescending(g => g.NgayCap)
            .ToList();

        if (!cungHang.Any())
        {
            result.LyDo = $"Chưa có GPLX hạng {dieuKien.HangBatBuocId}";
            return result;
        }

        foreach (var giayPhep in cungHang)
        {
            if (LaGiayPhepThoaDieuKien(giayPhep, dieuKien.NamToiThieu, today, out var lyDo))
            {
                result.DuDieuKien = true;
                result.LyDo = null;
                GanThongTinGiayPhep(result, giayPhep);
                return result;
            }

            result.LyDo ??= lyDo;
            GanThongTinGiayPhep(result, giayPhep);
        }

        return result;
    }

    private static bool LaGiayPhepThoaDieuKien(
        Giayphep giayPhep,
        int namToiThieu,
        DateOnly today,
        out string lyDo)
    {
        if (giayPhep.TrangThai == TrangThaiChoDuyet)
        {
            lyDo = "GPLX đang chờ duyệt";
            return false;
        }

        if (giayPhep.TrangThai == TrangThaiBiThuHoi || (giayPhep.SoDiem ?? 0) <= 0)
        {
            lyDo = "GPLX đã bị thu hồi hoặc còn 0 điểm";
            return false;
        }

        if (giayPhep.TrangThai == TrangThaiHetHan ||
            giayPhep.TrangThai == "Hết thời hạn" ||
            (giayPhep.NgayHetHan.HasValue && giayPhep.NgayHetHan.Value < today))
        {
            lyDo = "GPLX đã hết hạn";
            return false;
        }

        if (!giayPhep.NgayCap.HasValue)
        {
            lyDo = "GPLX chưa có ngày cấp";
            return false;
        }

        var ngayDuNam = giayPhep.NgayCap.Value.AddYears(Math.Max(namToiThieu, 0));
        if (ngayDuNam > today)
        {
            lyDo = $"GPLX chưa đủ {namToiThieu} năm kể từ ngày cấp";
            return false;
        }

        lyDo = string.Empty;
        return true;
    }

    private static void GanThongTinGiayPhep(HoSoDieuKienHangDTO result, Giayphep giayPhep)
    {
        result.SoGiayPhep = giayPhep.SoGiayPhep;
        result.NgayCap = giayPhep.NgayCap;
        result.NgayHetHan = giayPhep.NgayHetHan;
        result.SoDiem = giayPhep.SoDiem;
        result.TrangThai = giayPhep.TrangThai;
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
