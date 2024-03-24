using AutoMapper;
using LogisticsCompany.Dto;
using LogisticsCompany.Request;

namespace LogisticsCompany.Mapping.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestModel, RegisterDto>();
        }
    }
}
