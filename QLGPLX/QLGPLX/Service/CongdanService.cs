using AutoMapper;
using Backend.Repository;
using Backend.DTO.Congdan;
using Backend.Service.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using QLGPLX.Models;
namespace Backend.Service;
public class CongdanService : ICongDanService
{
    private readonly CongdanRepository _repo;
    private readonly IMapper _mapper;
    private readonly ICloudinaryService _cloudService;

    public CongdanService(CongdanRepository repo, IMapper mapper, ICloudinaryService cloudinaryService)
    {
        _repo = repo;
        _mapper = mapper;
        _cloudService = cloudinaryService;
 
    }
    public async Task Create(CreateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham)
    {
        try
        {
            var congdan = _mapper.Map<Congdan>(dto);
            congdan.PublicId = Guid.NewGuid();
            congdan.NgayTao = DateTime.Now;

            // ===== AVATAR =====
            if (anh3x4 != null)
            {
                congdan.Anh3x4 = await _cloudService.UploadImageAsync(
                    anh3x4,
                    "QLGPLX/avatar",
                    congdan.PublicId.ToString()
                );
            }

            // ===== GIẤY KHÁM =====
            if (giayKham != null)
            {
                congdan.GiayKhamSucKhoe = await _cloudService.UploadImageAsync(
                    giayKham,
                    "QLGPLX/gksk",
                    congdan.PublicId.ToString()
                );
            }

            _repo.Add(congdan);
           
        }
        catch (DbUpdateException ex)
        {
            var inner = ex.InnerException?.Message ?? "";

            if (inner.Contains("Duplicate entry"))
            {
                if (inner.Contains("CCCD"))
                    throw new Exception("CCCD đã tồn tại");

                if (inner.Contains("Email"))
                    throw new Exception("Email đã tồn tại");

                if (inner.Contains("SoDienThoai"))
                    throw new Exception("SĐT đã tồn tại");

                throw new Exception("Dữ liệu bị trùng");
            }

            throw;
        }
    }

    public async Task Update(Guid id, UpdateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham)
    {
        try
        {
            var congdan = _repo.GetById(id);
            if (congdan == null) return;

            _mapper.Map(dto, congdan);

            // ===== AVATAR =====
            if (anh3x4 != null)
            {           
                congdan.Anh3x4 = await _cloudService.UploadImageAsync(
                    anh3x4,
                    "QLGPLX/avatar",
                    congdan.PublicId.ToString()
                );
            }

            // ===== GIẤY KHÁM =====
            if (giayKham != null)
            {
                congdan.GiayKhamSucKhoe = await _cloudService.UploadImageAsync(
                    giayKham,
                    "QLGPLX/gksk",
                    congdan.PublicId.ToString()
                );
            }
            _repo.Update(congdan);
        }
        catch (DbUpdateException ex)
        {
            var inner = ex.InnerException?.Message ?? "";

            if (inner.Contains("Duplicate entry"))
            {
                if (inner.Contains("CCCD"))
                    throw new Exception("CCCD đã tồn tại");

                if (inner.Contains("Email"))
                    throw new Exception("Email đã tồn tại");

                if (inner.Contains("SoDienThoai"))
                    throw new Exception("SĐT đã tồn tại");

                throw new Exception("Dữ liệu bị trùng");
            }

            throw;
        }
    }


    public void Delete(Guid id)
    {
        var congdan = _repo.GetById(id);
        if (congdan != null) _repo.Delete(congdan);
    }

    public List<CongdanDTO> GetAll()
    {
        var data = _repo.GetAll();
        return _mapper.Map<List<CongdanDTO>>(data);
    }

    public CongdanDTO? GetById(Guid id)
    {
        var congdan = _repo.GetById(id);
        return congdan == null ? null : _mapper.Map<CongdanDTO>(congdan);
    }

    public async Task<List<CongdanDTO>> GetChuaCoHoSo()
    {
        var data = await _repo.GetCongDanChuaCoHoSo();
        return _mapper.Map<List<CongdanDTO>>(data);
    }

    public async Task<List<CongdanDTO>> GetHomNay()
    {
        var data = await _repo.GetCongDanHomNay();
        return _mapper.Map<List<CongdanDTO>>(data);
    }

    public async Task<List<CongdanDTO>> SearchCCCD(string cccd)
    {
        var data = await _repo.SearchByCCCD(cccd);
        return _mapper.Map<List<CongdanDTO>>(data);
    }

}