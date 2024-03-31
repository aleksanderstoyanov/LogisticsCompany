namespace LogisticsCompany.Services.Dto
{
    public class LoginDto
    {
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string PasswordHash { get; set; }
    }
}
