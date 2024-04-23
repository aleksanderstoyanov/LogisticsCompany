using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsCompany.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int OfficeId { get; set; }
        public string PasswordHash { get; set; }
    }
}
