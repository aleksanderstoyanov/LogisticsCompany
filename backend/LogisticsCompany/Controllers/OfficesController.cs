using AutoMapper;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Office;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var offices = await _officeService.GetAll();
            return Ok(offices);
        }
    }
}
