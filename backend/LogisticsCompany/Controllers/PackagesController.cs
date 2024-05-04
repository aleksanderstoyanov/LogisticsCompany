using AutoMapper;
using LogisticsCompany.Request.Package;
using LogisticsCompany.Response.Package;
using LogisticsCompany.Services.Package.Commands;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Package.Queries;
using Microsoft.AspNetCore.Mvc;

using static LogisticsCompany.Helpers.AuthorizationRequestHelper;

namespace LogisticsCompany.Controllers
{
    /// <summary>
    /// A <see cref="ControllerBase"/> which will handle the requests made based on package operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPackageQueryService _queryService;
        private readonly IPackageCommandService _commandService;

        /// <summary>
        /// Creates a <see cref="PackagesController"/> controller instance 
        /// with the injected <paramref name="mapper"/>, <paramref name="queryService"/>, and <paramref name="commandService"/> arguments.
        /// </summary>
        /// <param name="mapper">Mapper used for transpoing existing Request to DTO models.</param>
        /// <param name="queryService">Service used for executing query operations to the database.</param>
        /// <param name="commandService">Service used for executing command operations to the database.</param>
        public PackagesController(IMapper mapper, 
            IPackageQueryService queryService, 
            IPackageCommandService commandService)
        {
            _mapper = mapper;
            _queryService = queryService;
            _commandService = commandService;
        }

        /// <summary>
        /// Action Method which will intercept the incoming request made for retrieving all packages.
        /// </summary>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// Action Method which will handle the request made for retrieving all packages received for a given client.
        /// </summary>
        /// <param name="id">Query String argument for the Package Id</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming request made for retrieving all packages sent from a given client.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming request made for creating an existing Package Entity.
        /// </summary>
        /// <param name="requestModel">The Model coming from the Request Body</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// The Action Method which will intercept the incoming PUT request for updating an existing package entity.
        /// </summary>
        /// <param name="requestModel">The Model coming from the Request Body</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// The Action Method which will intercept the incoming DELETE request for deleting an existing package entity.
        /// </summary>
        /// <param name="id">The Query String parameter for the Package Id.</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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
