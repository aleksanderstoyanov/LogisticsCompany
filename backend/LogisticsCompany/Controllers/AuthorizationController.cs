using AutoMapper;
using LogisticsCompany.Request.Authorization;
using LogisticsCompany.Services.Authorization;
using LogisticsCompany.Services.Authorization.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsCompany.Controllers
{
    /// <summary>
    /// A <see cref="ControllerBase"/> which will handle requests made for register and login operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Create a <see cref="AuthorizationController" /> instance 
        /// with the injected <paramref name="authorizationService"/>, <paramref name="configuration"/>, <paramref name="mapper"/> arguments.
        /// </summary>
        /// <param name="mapper">Mapper used for mapping Request to DTO models.</param>
        /// <param name="authorizationService">Service which will perform Login and Register operations to the database.</param>
        /// <param name="configuration">Configuration used for gathering the application's settings.</param>
        public AuthorizationController(IMapper mapper, IAuthorizationService authorizationService, IConfiguration configuration)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Action Method which will intercept the incoming POST request for registration.
        /// </summary>
        /// <param name="requestModel">The Model coming from the Request Body.</param>
        /// <returns> <see cref="ObjectResult"/> for the response.</returns>
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

        /// <summary>
        /// Action Method which will intercept the incoming POST request for login.
        /// </summary>
        /// <param name="requestModel">The model coming from the request body.</param>
        /// <returns> <see cref="ObjectResult"/> for the response.</returns>
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
