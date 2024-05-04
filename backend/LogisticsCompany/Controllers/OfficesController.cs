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
    [ApiController]
    [Route("/api/[controller]")]

    public class OfficesController : ControllerBase
    {
        private readonly IOfficeQueryService _queryService;
        private readonly IOfficeCommandService _commandService;
        private readonly IMapper _mapper;

        public OfficesController(IMapper mapper,
            IOfficeQueryService queryService,
            IOfficeCommandService commandService)
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
            var header = HttpContext.Request.Headers.Authorization;

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header) && !AuthorizationRequestHelper.IsAuthorized("Client", header))
            {
                return Unauthorized();
            }

            var offices = await _queryService.GetAll();
            return Ok(offices);
        }

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
