
using Backend.DTO.HangGiayPhep;
using Backend.Service.Interface;
using Backend.Repository;


namespace Backend.Service;

public class HangGiayPhepService : IHangGiayPhepService
{
    private readonly HangGiayPhepRepository _repository;

    public HangGiayPhepService(HangGiayPhepRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<HangGiayPhepDTO>> GetAllAsync()
    {
        var hangs = await _repository.GetAllAsync();
        return hangs.Select(h => new HangGiayPhepDTO
        {
            MaHang = h.MaHang,
            TenHang = h.TenHang,
            LoaiXe = h.LoaiXe,
            DoTuoiToiThieu = h.DoTuoiToiThieu,
            ThoiHanNam = h.ThoiHanNam,
            YeuCauThucHanh = h.YeuCauThucHanh,
            MoTaChiTiet = h.MoTaChiTiet
        }).ToList();
    }

    public async Task<HangGiayPhepDTO?> GetByIdAsync(string maHang)
    {
        var hang = await _repository.GetByIdAsync(maHang);
        if (hang == null) return null;

        return new HangGiayPhepDTO
        {
            MaHang = hang.MaHang,
            TenHang = hang.TenHang,
            LoaiXe = hang.LoaiXe,
            DoTuoiToiThieu = hang.DoTuoiToiThieu,
            ThoiHanNam = hang.ThoiHanNam,
            YeuCauThucHanh = hang.YeuCauThucHanh,
            MoTaChiTiet = hang.MoTaChiTiet
        };
    }
}