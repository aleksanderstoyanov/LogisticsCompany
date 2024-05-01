using AutoMapper;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IMapper _mapper;

        public DeliveriesController(IDeliveryService deliveryService, IMapper mapper)
        {
            this._deliveryService = deliveryService;
            this._mapper = mapper;
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

            var deliveries = await _deliveryService.GetAll();

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

            var statusMessage = await _deliveryService.Create(dto);

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

            await _deliveryService.Update(dto);

            return Ok();
        }
    }
}
