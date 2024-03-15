using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManagement.Models
{
    public class Footballer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string AgeCategory { get; set; } = String.Empty;
        public string? Country { get; set; } = string.Empty;
        //[Display(Name = "Wiek")]
        public DateTime? DateOfBirth { get; set; }
        public float? Growth { get; set; }
        public float? Weight { get; set; }
        public string? Position { get; set; } = string.Empty;
        public int? PlayerNumber { get; set; }
        public string? WhichFoot { get; set; } = string.Empty;

        // Footballer 1-1 Account
        public int? AccountId { get; set; }
        public Account Account { get; set; }
        // Footballer 1-1 Statistics
        //public int? StatisticsId { get; set; } // Niepotrzebne do relacji rodzic dziecko
        public Statistics Statistics { get; set; }

        // Footballer 1-* IndividualTraining
        public ICollection<IndividualTraining> IndividualTrainings { get; set; } = new List<IndividualTraining>();
        // Footballer *-* GroupTraining
        public ICollection<GroupTraining> GroupTrainings { get; set; } = new List<GroupTraining>();
        // Footballer *-* Match
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
    
