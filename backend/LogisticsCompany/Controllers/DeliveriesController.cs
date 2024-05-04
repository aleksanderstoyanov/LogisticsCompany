using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LogisticsCompany.Helpers;
using LogisticsCompany.Services.Deliveries.Commands;
using LogisticsCompany.Services.Deliveries.Queries;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using LogisticsCompany.Request.Delivery;

namespace LogisticsCompany.Controllers
{
    /// <summary>
    /// A <see cref="ControllerBase" /> which will handle the request made upon delivery operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryQueryService _queryService;
        private readonly IDeliveryCommandService _commandService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a <see cref="DeliveriesController"/> instance
        /// with the injected <paramref name="mapper"/>, <paramref name="queryService"/>, and <paramref name="commandService"/> arguments.
        /// </summary>
        /// <param name="mapper">Mapper used for transpoing existing Request to DTO models.</param>
        /// <param name="queryService">Service used for executing query operations to the database.</param>
        /// <param name="commandService">Service used for executing command operations to the database.</param>
        public DeliveriesController(IMapper mapper,
            IDeliveryQueryService queryService,
            IDeliveryCommandService commandService)
        {
            _mapper = mapper;
            _queryService = queryService;
            _commandService = commandService;
        }

        /// <summary>
        /// Action Method which will intercept the incoming GET request made for retrieving all deliveries.
        /// </summary>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
        [HttpGet]
        [Authorize]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Courier", header))
            {
                return Unauthorized();
            }

            var deliveries = await _queryService.GetAll();

            return Ok(deliveries);
        }

        /// <summary>
        /// Action Method which will intercept the incoming POST request made for creating a new delivery entity.
        /// </summary>
        /// <param name="requestModel">The incoming model from the Request Body.</param>
        /// <returns>
        /// <see cref="ObjectResult" /> for the response.
        /// </returns>
        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create(DeliveryCreateRequest requestModel)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Courier", header))
            {
                return Unauthorized();
            }

            var dto = _mapper.Map<DeliveryCreateRequest, DeliveryDto>(requestModel);

            var statusMessage = await _commandService.Create(dto);

            return Ok(statusMessage);
        }

        /// <summary>
        /// Action Method which will intercept the incoming PUT request made for updating an existing delivery entity.
        /// </summary>
        /// <param name="requestModel">The incoming model from the Request Body</param>
        /// <returns>
        /// <see cref="ObjectResult" /> for the response.
        /// </returns>
        [HttpPut]
        [Authorize]
        [Route("update")]
        public async Task<IActionResult> Update(DeliveryUpdateRequest requestModel)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Courier", header))
            {
                return Unauthorized();
            }

            var dto = _mapper.Map<DeliveryUpdateRequest, DeliveryDto>(requestModel);

            await _commandService.Update(dto);

            return Ok();
        }
    }
}
