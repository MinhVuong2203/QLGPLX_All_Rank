using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DTO.Congdan;
using QLGPLX.Models;
using QLGPLX.Repository;

public class CongdanService : ICongDanService
{
    private readonly CongdanRepository _repo;
    private readonly IMapper _mapper;
    private readonly Cloudinary _cloudinary;

    public CongdanService(CongdanRepository repo, IMapper mapper, IConfiguration config)
    {
        _repo = repo;
        _mapper = mapper;
       
        var acc = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(acc);
    }
    public async Task Create(CreateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham)
    {
        var congdan = _mapper.Map<Congdan>(dto);
        congdan.PublicId = Guid.NewGuid();
        congdan.NgayTao = DateTime.Now;
        if (anh3x4 != null) congdan.Anh3x4 = await UploadFile(anh3x4);
        if (giayKham != null) congdan.GiayKhamSucKhoe = await UploadFile(giayKham);
        _repo.Add(congdan); 
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

    public async Task Update(Guid id, UpdateCongdanDTO dto)
    {
        var congdan = _repo.GetById(id);
        if (congdan == null) return;
        _mapper.Map(dto, congdan);
        _repo.Update(congdan);
    }

    private async Task<string> UploadFile(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream)
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl.ToString();
    }

}