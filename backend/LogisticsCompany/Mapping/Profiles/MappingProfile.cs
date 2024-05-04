using AutoMapper;
using LogisticsCompany.Request.Authorization;
using LogisticsCompany.Request.Delivery;
using LogisticsCompany.Request.Office;
using LogisticsCompany.Request.Package;
using LogisticsCompany.Request.User;
using LogisticsCompany.Response.Office;
using LogisticsCompany.Response.Package;
using LogisticsCompany.Services.Authorization.Dto;
using LogisticsCompany.Services.Dto;
using LogisticsCompany.Services.Offices.Dto;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Users.Dto;

namespace LogisticsCompany.Mapping.Profiles
{
    /// <summary>
    /// Mapping class used for providing a scoped profile of mapping configurations.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Creates a Mapping Profile with defined mappings configurations.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<RegisterRequestModel, RegisterDto>();
            CreateMap<LoginRequestModel, LoginDto>();
            CreateMap<UserRequestModel, UserDto>();
            
            CreateMap<OfficeRequestModel, OfficeDto>();
            CreateMap<OfficeCreateRequestModel, OfficeDto>();
            CreateMap<OfficeDto, OfficeResponseModel>();

            CreateMap<PackageDto, PackageClientResponseModel>();
            CreateMap<SentReceivedPackageDto, PackageClientResponseModel>();
            CreateMap<PackageRequestModel, PackageDto>();

            CreateMap<DeliveryCreateRequest, DeliveryDto>();
            CreateMap<DeliveryUpdateRequest, DeliveryDto>();
        }
    }
}
