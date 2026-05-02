
using Backend.DTO.Congdan;

namespace Backend.Service.Interface;
public interface ICongDanService
{
    List<CongdanDTO> GetAll();
    CongdanDTO? GetById(Guid id);
    Task Create(CreateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham);
    Task Update(Guid id, UpdateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham);
    void Delete(Guid id);
}