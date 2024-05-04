using AutoMapper;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request;
using LogisticsCompany.Response;
using LogisticsCompany.Services.Users.Commands;
using LogisticsCompany.Services.Users.Dto;
using LogisticsCompany.Services.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static LogisticsCompany.Helpers.AuthorizationRequestHelper;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserQueryService _queryService;
        private readonly IUserCommandService _commandService;
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper, 
            IUserQueryService queryService,
            IUserCommandService commandService)
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

            if (!IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            // TO DO: Add rewiring and mapping to response model.
            var users = await _queryService.GetUsers();

            return Ok(users);
        }

        [HttpGet]
        [Authorize]
        [Route("getAllExcept")]
        public async Task<IActionResult> GetAllExcept(int id, string role)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("Client", header) && !IsAuthorized("OfficeEmployee", header) && !IsAuthorized("Courier", header))
            {
                return Unauthorized();
            }

            var users = await _queryService.GetDifferentUsersFromCurrent(id, role);

            return Ok(users);

        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public async Task<IActionResult> Update(UserRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var dto = _mapper.Map<UserRequestModel, UserDto>(requestModel);

            await _commandService.Update(dto);

            return Ok("");

        }

        [HttpDelete]
        [Authorize]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var header = HttpContext.Request.Headers["Authorization"];

            if (!IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            await _commandService.Delete(id);

            return Ok("");

        }
    }
}
