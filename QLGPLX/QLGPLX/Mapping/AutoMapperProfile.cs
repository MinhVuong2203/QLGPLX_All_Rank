using AutoMapper;
using DTO.Congdan;
using QLGPLX.Models;

namespace QLGPLX.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Công dân
            CreateMap<Congdan, CongdanDTO>();
            CreateMap<CreateCongdanDTO, Congdan>();
            CreateMap<UpdateCongdanDTO, Congdan>();
        }
    }
}
