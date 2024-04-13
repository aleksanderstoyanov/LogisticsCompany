using AutoMapper;
using LogisticsCompany.Helpers;
using LogisticsCompany.Request;
using LogisticsCompany.Response;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace LogisticsCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this._userService = userService;
            this._mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        [Route("getAll")]
        public async Task <IActionResult> GetAll()
        {
            var authorization = HttpContext.Request.Headers["Authorization"].ToString();

            authorization = authorization.Replace("Bearer", string.Empty);
            authorization = authorization.Replace(" ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();

            var parsedToken = tokenHandler.ReadJwtToken(authorization);
            var roleClaim = parsedToken
                    .Claims
                    .FirstOrDefault(claim => claim.Type == "Role")
                    .Value;

            if(roleClaim != "Admin")
            {
                return Unauthorized();
            }

            // TO DO: Add rewiring and mapping to response model.
            var users = await this._userService.GetUsers();

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

            if (!AuthorizationRequestHelper.IsAuthorized("Admin", header))
            {
                return Unauthorized();
            }

            var dto = _mapper.Map<UserRequestModel, UserDto>(requestModel);

            await _userService.Update(dto);

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

            if (!AuthorizationRequestHelper.IsAuthorized("Admin",  header))
            {
                return Unauthorized();
            }

            await _userService.Delete(id);

            return Ok("");

        }
    }
}
