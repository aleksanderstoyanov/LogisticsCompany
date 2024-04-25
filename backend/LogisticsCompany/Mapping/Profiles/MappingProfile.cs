using AutoMapper;
using LogisticsCompany.Dto;
using LogisticsCompany.Request;
using LogisticsCompany.Response;
using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Mapping.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestModel, RegisterDto>();
            CreateMap<LoginRequestModel, LoginDto>();
            CreateMap<UserRequestModel, UserDto>();
            
            CreateMap<OfficeRequestModel, OfficeDto>();
            CreateMap<OfficeCreateRequestModel, OfficeDto>();
            CreateMap<OfficeDto, OfficeResponseModel>();

            CreateMap<PackageDto, PackageClientResponseModel>();
            CreateMap<PackageRequestModel, PackageDto>();
        }
    }
}
