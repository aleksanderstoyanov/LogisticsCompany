﻿using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request
{
    public class RegisterRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "Client";
    }
}
