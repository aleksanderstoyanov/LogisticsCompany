﻿using AutoMapper;
using LogisticsCompany.Dto;
using LogisticsCompany.Request;
using LogisticsCompany.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthorizationController(IUserService userService, IMapper mapper)
        {
            this._userService = userService;
            this._mapper = mapper;
        }

        [Route("register")]
        [HttpPost]
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
    }
}