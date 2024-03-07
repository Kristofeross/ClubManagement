using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManagement.Models
{
    public class Coach
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        // Coach 1-1 Account
        public int AccountId { get; set; }
        public Account Account { get; set; }

    }
}
