using Backend.Models;
using Backend.Repository;
using Backend.Service.Interface;

namespace Backend.Service
{
    public class ChucNangService : IChucNangService
    {
        private readonly ChucNangRepository _chucNangRepository;

        public ChucNangService(ChucNangRepository chucNangRepository)
        {
            _chucNangRepository = chucNangRepository;
        }

        public async Task<List<Chucnang>> GetAllAsync()
        {
            return await _chucNangRepository.GetAllAsync();
        }
    }
}
