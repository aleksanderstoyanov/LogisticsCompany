using LogisticsCompany.Helpers;
using LogisticsCompany.Response.Report;
using LogisticsCompany.Services.Reports.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCompany.Controllers
{
    /// <summary>
    /// A <see cref="ControllerBase"/> which will handle request made based on report operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportQueryService _queryService;

        /// <summary>
        /// Creates a <see cref="ReportsController" /> instance 
        /// with the injected <paramref name="queryService"/> argument.
        /// </summary>
        /// <param name="queryService">Service used for executing query operations to the database.</param>
        public ReportsController(IReportQueryService queryService)
        {
            _queryService = queryService;
        }

        /// <summary>
        /// Action Method which will intercept the incoming GET request made for retrieving all users that are employees.
        /// </summary>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming GET request made for retrieving all users that are clients.
        /// </summary>
        /// <returns>
        /// <see cref="ObjectResult"/>
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming GET request made for retrieving all registered packages.
        /// </summary>
        /// <returns>
        /// <see cref="ObjectResult"/>
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming GET request made for retrieving all non-delivered packages.
        /// </summary>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming GET request made for retrieving all non-delivered packages.
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="endPeriod"></param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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
