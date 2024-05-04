using AutoMapper;
using LogisticsCompany.Entity;
using LogisticsCompany.Request;
using LogisticsCompany.Services.Authorization;
using LogisticsCompany.Services.Authorization.Dto;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public AuthorizationController(IMapper mapper, IAuthorizationService authorizationService, IConfiguration configuration)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _configuration = configuration;
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
            _authorizationService.Register(dto);

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

            var token = await _authorizationService.Login(dto, issuer, key);

            if (token.IsNullOrEmpty())
            {
                return BadRequest("Invalid Crendetials");
            }

            return Ok(token);
        }
    }
}
