using AutoMapper;
using Backend.DTO.Congdan;
using Backend.DTO.HoSo;
using Backend.DTO.KyThi;
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

            // Entity -> DTO
            CreateMap<Kythi, KyThiDTO>()
                .ForMember(dest => dest.TenHang,
                    opt => opt.MapFrom(src => src.MaHangNavigation != null
                        ? src.MaHangNavigation.TenHang
                        : null));

            // ========== Kỳ thi ==========
            // Kỳ thi mappings
            CreateMap<Kythi, KyThiDTO>()
                .ForMember(dest => dest.PublicId, opt => opt.MapFrom(src => src.PublicId))
                .ForMember(dest => dest.TenHang, opt => opt.Ignore());
               

            CreateMap<CreateKyThiDTO, Kythi>()
                .ForMember(dest => dest.KyThiId, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.Ignore());

            CreateMap<UpdateKyThiDTO, Kythi>()
                .ForMember(dest => dest.KyThiId, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.Ignore());

        }
    }
}
