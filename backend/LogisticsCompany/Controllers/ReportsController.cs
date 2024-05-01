using LogisticsCompany.Helpers;
using LogisticsCompany.Services;
using LogisticsCompany.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            this._reportService = reportService;
        }


        [HttpGet]
        [Authorize]
        [Route("allEmployees")]
        public async Task<IActionResult> AllEmployees()
        {
            var employees = await _reportService.GetAllEmployees();

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
            var clients = await _reportService.GetAllClients();

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
            var packages = await _reportService.GetAllRegisteredPackages();

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
        [Route("allInDeliveryPackages")]
        public async Task<IActionResult> GetAllNonDeliveredPackages()
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var packages = await _reportService.GetAllInDeliveryPackages();

            return Ok(new
            {
                Data = packages,
                DataFor = "NonDeliveredPackages",
            });
        }
    }
}
