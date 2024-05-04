namespace LogisticsCompany.Services.Authorization.Dto
{
    public class LoginDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string PasswordHash { get; set; }
    }
}
