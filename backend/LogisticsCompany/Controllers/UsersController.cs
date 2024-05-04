using AutoMapper;
using LogisticsCompany.Request.User;
using LogisticsCompany.Services.Users.Commands;
using LogisticsCompany.Services.Users.Dto;
using LogisticsCompany.Services.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static LogisticsCompany.Helpers.AuthorizationRequestHelper;

namespace LogisticsCompany.Controllers
{
    /// <summary>
    /// A <see cref="ControllerBase" /> which will handle request made for user operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserQueryService _queryService;
        private readonly IUserCommandService _commandService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a <see cref="UsersController" /> instance 
        /// with the injected <paramref name="mapper"/>, <paramref name="queryService"/>, and <paramref name="commandService"/>
        /// </summary>
        /// <param name="mapper">Mapper used for transposing Request to Dto models.</param>
        /// <param name="queryService">Service used for performing query operations to the database.</param>
        /// <param name="commandService">Service used for performing command operations to the database.</param>
        public UsersController(IMapper mapper, 
            IUserQueryService queryService,
            IUserCommandService commandService)
        {
            _mapper = mapper;
            _queryService = queryService;
            _commandService = commandService;
        }

        /// <summary>
        /// Action Method which will intercept the incoming GET request for retrieving all users.
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

            if (!IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            // TO DO: Add rewiring and mapping to response model.
            var users = await _queryService.GetUsers();

            return Ok(users);
        }

        /// <summary>
        /// Action Method which will intercept the incoming GET request for retrieving all users except one.
        /// </summary>
        /// <param name="id">The Query String paramater for the UserId.</param>
        /// <param name="role">The Query String parameter for the User Role.</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming PUT request for updating an existing User entity.
        /// </summary>
        /// <param name="requestModel">The Model coming from the Request Body</param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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

        /// <summary>
        /// Action Method which will intercepth the incoming DELETE request for deleting an existing User entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <see cref="ObjectResult"/> for the response.
        /// </returns>
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
