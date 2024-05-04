using AutoMapper;
using LogisticsCompany.Request;
using LogisticsCompany.Response;
using LogisticsCompany.Services.Package.Commands;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Package.Queries;
using Microsoft.AspNetCore.Mvc;

using static LogisticsCompany.Helpers.AuthorizationRequestHelper;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPackageQueryService _queryService;
        private readonly IPackageCommandService _commandService;

        public PackagesController(IMapper mapper, 
            IPackageQueryService queryService, 
            IPackageCommandService commandService)
        {
            _mapper = mapper;
            _queryService = queryService;
            _commandService = commandService;
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

            var result = await _queryService.GetAll();

            return Ok(result);
        }

        [HttpGet]
        [Route("getReceived")]
        public async Task<IActionResult> GetReceived(int id)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("Client", header) && !IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var result = await _queryService.GetReceivedPackages(id);
            var response = _mapper.Map<IEnumerable<SentReceivedPackageDto>, IEnumerable<PackageClientResponseModel>>(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("getSent")]
        public async Task<IActionResult> GetSent(int id)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if(!IsAuthorized("Client", header) && !IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var result = await _queryService.GetSentPackages(id);
            var response = _mapper.Map<IEnumerable<SentReceivedPackageDto>, IEnumerable<PackageClientResponseModel>>(result);

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

            await _commandService.Create(dto);

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

            await _commandService.Update(dto);

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

            await _commandService.Delete(id);

            return Ok();
        }
    }
}
