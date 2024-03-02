using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class AccountRegisterDto
    {
        /*public string AccountName { get; set; } = string.Empty;*/
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string ConfirmedPasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; } = 1;
    }
}
