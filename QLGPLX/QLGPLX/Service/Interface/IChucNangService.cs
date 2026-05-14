using Backend.Models;

namespace Backend.Service.Interface
{
    public interface IChucNangService
    {
        Task<List<Chucnang>> GetAllAsync();
    }
}
