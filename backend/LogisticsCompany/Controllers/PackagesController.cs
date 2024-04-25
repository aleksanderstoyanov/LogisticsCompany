using AutoMapper;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPackageService _packageService;

        public PackagesController(IMapper mapper, IPackageService packageService)
        {
            this._mapper = mapper;
            this._packageService = packageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(PackageRequestModel requestModel)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Client", header))
            {
                return Unauthorized();
            }

            var dto = _mapper.Map<PackageRequestModel, PackageDto>(requestModel);

            await _packageService.Create(dto);

            return Ok();
        }
    }
}
