using AutoMapper;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request.Office;
using LogisticsCompany.Response.Office;
using LogisticsCompany.Services.Offices.Commands;
using LogisticsCompany.Services.Offices.Dto;
using LogisticsCompany.Services.Offices.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCompany.Controllers
{
    /// <summary>
    /// A <see cref="ControllerBase"/> which will handle request made for office operations.
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]

    public class OfficesController : ControllerBase
    {
        private readonly IOfficeQueryService _queryService;
        private readonly IOfficeCommandService _commandService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a <see cref="OfficesController"/> instance 
        /// with the injected <paramref name="mapper"/>, <paramref name="queryService"/>, and <paramref name="commandService"/>
        /// arguments.
        /// </summary>
        /// <param name="mapper">Mapper used for transpoing existing Request to DTO models.</param>
        /// <param name="queryService">Service used for executing query operations to the database.</param>
        /// <param name="commandService">Service used for executing command operations to the database.</param>
        public OfficesController(IMapper mapper,
            IOfficeQueryService queryService,
            IOfficeCommandService commandService)
        {
            _mapper = mapper;
            _queryService = queryService;
            _commandService = commandService;
        }

        /// <summary>
        /// Action Method which will intercept the incoming request for gathering all office instances.
        /// </summary>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
        [HttpGet]
        [Authorize]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var header = HttpContext.Request.Headers.Authorization;

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header) && !AuthorizationRequestHelper.IsAuthorized("Client", header))
            {
                return Unauthorized();
            }

            var offices = await _queryService.GetAll();
            return Ok(offices);
        }

        /// <summary>
        /// Action Method which will intercept the incoming POST request for creating a new office entity.
        /// </summary>
        /// <param name="requestModel">The Model coming from the Request Body.</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the result.
        /// </returns>
        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create(OfficeCreateRequestModel requestModel)
        {
            var header = this.HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var dto = this._mapper.Map<OfficeCreateRequestModel, OfficeDto>(requestModel);

            var office = await _commandService.Create(dto);

            if (office == null)
            {
                return BadRequest();
            }

            var response = _mapper.Map<OfficeDto, OfficeResponseModel>(office);

            return Ok(response);
        }

        /// <summary>
        /// Action Method which will intercept the incoming PUT request for updating an existing office entity.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
        [HttpPut]
        [Authorize]
        [Route("update")]
        public async Task<IActionResult> Update(OfficeRequestModel requestModel)
        {
            var header = this.HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var dto = this._mapper.Map<OfficeRequestModel, OfficeDto>(requestModel);

            await _commandService.Update(dto);

            return Ok();
        }

        /// <summary>
        /// Action Method which will intercept the incoming Delete request for deleting an existing office entity.
        /// </summary>
        /// <param name="id">Query String parameter for the Office Id.</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
        [HttpDelete]
        [Authorize]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var header = HttpContext.Request.Headers["Authorization"];
            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            await _commandService.Delete(id);

            return Ok();
        }
    }
}
