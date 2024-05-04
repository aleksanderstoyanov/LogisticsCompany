using LogisticsCompany.Helpers;
using LogisticsCompany.Response.Report;
using LogisticsCompany.Services.Reports.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportQueryService _queryService;

        public ReportsController(IReportQueryService queryService)
        {
            _queryService = queryService;
        }


        [HttpGet]
        [Authorize]
        [Route("allEmployees")]
        public async Task<IActionResult> AllEmployees()
        {
            var employees = await _queryService.GetAllEmployees();

            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Data = employees,
                DataFor = "Employees"
            });
        }

        [HttpGet]
        [Authorize]
        [Route("allClients")]
        public async Task<IActionResult> AllClients()
        {
            var clients = await _queryService.GetAllClients();

            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Data = clients,
                DataFor = "Clients"
            });
        }

        [HttpGet]
        [Authorize]
        [Route("allRegisteredPackages")]
        public async Task<IActionResult> AllRegisteredPackages()
        {
            var packages = await _queryService.GetAllRegisteredPackages();

            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Data = packages,
                DataFor = "RegisteredPackages"
            });
        }

        [HttpGet]
        [Authorize]
        [Route("allInDeliveryPackages")]
        public async Task<IActionResult> GetAllNonDeliveredPackages()
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var packages = await _queryService.GetAllInDeliveryPackages();

            return Ok(new
            {
                Data = packages,
                DataFor = "NonDeliveredPackages",
            });
        }

        [HttpGet]
        [Authorize]
        [Route("getIncomesForPeriod")]
        public async Task<IActionResult> GetIncomesForPeriod([FromQuery] DateTime startPeriod, [FromQuery] DateTime endPeriod)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            if (startPeriod > endPeriod)
            {
                return BadRequest("Start Period should be less than End Period!");
            }

            var income = await _queryService.GetIncomeForPeriod(startPeriod, endPeriod);

            return Ok(new ReportIncomeResponseModel
            {
                Income = income
            });

        }
    }
}
