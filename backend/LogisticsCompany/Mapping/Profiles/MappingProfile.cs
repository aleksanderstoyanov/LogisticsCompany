using AutoMapper;
using LogisticsCompany.Dto;
using LogisticsCompany.Request;
using LogisticsCompany.Services.Dto;

namespace LogisticsCompany.Mapping.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestModel, RegisterDto>();
            CreateMap<LoginRequestModel, LoginDto>();
        }
    }
}
