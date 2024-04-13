using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request
{
    public class UserRequestModel
    {
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
