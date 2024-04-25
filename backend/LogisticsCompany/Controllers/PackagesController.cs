using AutoMapper;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request;
using LogisticsCompany.Response;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Mvc;

using static LogisticsCompany.Helpers.AuthorizationRequestHelper;

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

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("OfficeEmployee", header) && !IsAuthorized("Courier", header))
            {
                return Unauthorized();
            }

            var result = await _packageService.GetAll();

            return Ok(result);
        }

        [HttpGet]
        [Route("getReceived")]
        public async Task<IActionResult> GetReceived(int id)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("Client", header))
            {
                return Unauthorized();
            }

            var result = await _packageService.GetReceivedPackages(id);
            var response = _mapper.Map<IEnumerable<PackageDto>, IEnumerable<PackageClientResponseModel>>(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("getSent")]
        public async Task<IActionResult> GetSent(int id)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if(!IsAuthorized("Client", header))
            {
                return Unauthorized();
            }

            var result = await _packageService.GetSentPackages(id);
            var response = _mapper.Map<IEnumerable<PackageDto>, IEnumerable<PackageClientResponseModel>>(result);

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(PackageRequestModel requestModel)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("Client", header))
            {
                return Unauthorized();
            }

            var dto = _mapper.Map<PackageRequestModel, PackageDto>(requestModel);

            await _packageService.Create(dto);

            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(PackageRequestModel requestModel)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("OfficeEmployee", header) && !IsAuthorized("Courier", header))
            {
                return Unauthorized();
            }

            var dto = _mapper.Map<PackageRequestModel, PackageDto>(requestModel);

            await _packageService.Update(dto);

            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("OfficeEmployee", header))
            {
                return Unauthorized();
            }

            await _packageService.Delete(id);

            return Ok();
        }
    }
}
