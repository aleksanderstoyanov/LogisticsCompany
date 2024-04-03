using AutoMapper;
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
            var roleClaim = parsedToken.Claims
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
    }
}
