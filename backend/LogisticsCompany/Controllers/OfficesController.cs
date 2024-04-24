using AutoMapper;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request;
using LogisticsCompany.Response;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]

    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly IMapper _mapper;

        public OfficesController(IOfficeService officeService, IMapper _mapper)
        {
            this._officeService = officeService;
            this._mapper = _mapper;
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

            var offices = await _officeService.GetAll();
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

            var office = await _officeService.Create(dto);

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

            await _officeService.Update(dto);

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

            await _officeService.Delete(id);

            return Ok();
        }
    }
}
