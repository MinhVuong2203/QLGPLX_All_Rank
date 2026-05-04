using AutoMapper;
using Backend.DTO.Congdan;
using Backend.DTO.HoSo;
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

            // Hồ sơ
            // Entity -> DTO
            CreateMap<Hoso, HosoDTO>()
                .ForMember(dest => dest.TenCongDan,
                    opt => opt.MapFrom(src => src.MaCongDanNavigation.HoTen))
                .ForMember(dest => dest.CCCD,
                    opt => opt.MapFrom(src => src.MaCongDanNavigation.Cccd))
                .ForMember(dest => dest.TenHang,
                    opt => opt.MapFrom(src => src.MaHangNavigation.TenHang));

            // Create
            CreateMap<CreateHosoDTO, Hoso>()
                .ForMember(dest => dest.HoSoId, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.Ignore())
                .ForMember(dest => dest.NgayNop, opt => opt.Ignore());

            // Update
            CreateMap<UpdateHosoDTO, Hoso>()
                .ForMember(dest => dest.HoSoId, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.Ignore())
                .ForMember(dest => dest.MaCongDan, opt => opt.Ignore())
                .ForMember(dest => dest.NgayNop, opt => opt.Ignore());
        }
    }
}
