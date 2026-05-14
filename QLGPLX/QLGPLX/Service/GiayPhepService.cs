using Backend.DTO.GiayPhep;
using Backend.Repository;
using Backend.Service.Interface;
using Backend.Models;

namespace Backend.Service
{
    public class GiayPhepService : IGiayPhepService
    {
        private readonly GiayPhepRepository _repository;

        public GiayPhepService(GiayPhepRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<GiayPhepDTO>> GetGiayPhepsAsync(GiayPhepSearchDTO search)
        {
            return await _repository.GetGiayPhepsAsync(search);
        }

        public async Task<GiayPhepDTO?> GetGiayPhepByIdAsync(int id)
        {
            return await _repository.GetGiayPhepByIdAsync(id);
        }

        public async Task<Giayphep> CreateGiayPhepAsync(GiayPhepCreateDTO dto)
        {
            return await _repository.CreateGiayPhepAsync(dto);
        }

        public async Task<bool> UpdateGiayPhepAsync(int id, GiayPhepUpdateDTO dto)
        {
            return await _repository.UpdateGiayPhepAsync(id, dto);
        }


        public async Task<object> GetStatisticsAsync()
        {
            return await _repository.GetStatisticsAsync();
        }
    }
}
