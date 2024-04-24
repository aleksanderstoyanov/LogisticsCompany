using System.ComponentModel.DataAnnotations;

namespace LogisticsCompany.Request
{
    public class UserRequestModel
    {
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string OfficeName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
