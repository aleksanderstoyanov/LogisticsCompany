using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request;
using LogisticsCompany.Services.Deliveries.Commands;
using LogisticsCompany.Services.Deliveries.Queries;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryQueryService _queryService;
        private readonly IDeliveryCommandService _commandService;
        private readonly IMapper _mapper;

        public DeliveriesController(IMapper mapper,
            IDeliveryQueryService queryService, 
            IDeliveryCommandService commandService)
        {
            _mapper = mapper;
            _queryService = queryService;
            _commandService = commandService;
        }

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
