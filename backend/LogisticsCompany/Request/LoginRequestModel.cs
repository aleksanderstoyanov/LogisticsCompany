using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
