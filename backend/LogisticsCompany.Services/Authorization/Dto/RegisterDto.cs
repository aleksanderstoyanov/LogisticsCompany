﻿namespace LogisticsCompany.Services.Authorization.Dto
{
    public class RegisterDto
    {
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Client";

    }
}
