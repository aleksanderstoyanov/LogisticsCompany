using AutoMapper;
using LogisticsCompany.Dto;
using LogisticsCompany.Entity;
using LogisticsCompany.Request;
using LogisticsCompany.Services.Contracts;
using LogisticsCompany.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogisticsCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthorizationController(IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            this._userService = userService;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = _mapper.Map<RegisterRequestModel, RegisterDto>(requestModel);
            _userService.Register(dto);

            return Ok("Registered Successfully");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var issuer = _configuration.GetSection("Jwt:Issuer").Get<string>();
            var key = _configuration.GetSection("Jwt:Key").Get<string>();

            var dto = _mapper.Map<LoginRequestModel, LoginDto>(requestModel);

            var token = await _userService.Login(dto, issuer, key);

            return Ok(token);
        }
    }
}
