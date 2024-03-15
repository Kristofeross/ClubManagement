using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManagement.Models
{
    public class Coach
    {
        [Key]
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Country { get; set; }
        public string? KindOfCoach { get; set; }
        public DateTime DateOfBirth { get; set; }
        // Coach 1-1 Account
        public Account Account { get; set; }
        // Coach 1-* IndividualTrainig
        public ICollection<IndividualTraining> IndividualTrainings { get; set; } = new List<IndividualTraining>();
        // Coach *-* GroupTraining
        public ICollection<GroupTraining> GroupTrainings { get; set; } = new List<GroupTraining>();
        // Coach *-* Match
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
