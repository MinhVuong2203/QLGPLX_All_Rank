using DTO.Congdan;

public interface ICongDanService
{
    List<CongdanDTO> GetAll();
    CongdanDTO? GetById(Guid id);
    Task Create(CreateCongdanDTO dto, IFormFile? anh3x4, IFormFile? giayKham);
    Task Update(Guid id, UpdateCongdanDTO dto);
    void Delete(Guid id);
}