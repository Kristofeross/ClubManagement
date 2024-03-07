using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManagement.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        // Account 1-1 Footballer
        public int? FootballerId { get; set; }
        public Footballer? Footballer { get; set; }
        // Account 1-1 Coach
        public int? CoachId { get; set; }
        public Coach? Coach { get; set; }
    }
}
