namespace ClubManagement.Models
{
    public class AccountLoginDto
    {
        public string Email { get; set; } = string.Empty;
        // Tu zmienić przed jakimś update-database na PasswordHash
        public string Password { get; set; } = string.Empty;
    }
}
