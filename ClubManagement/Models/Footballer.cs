using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManagement.Models
{
    public class Footballer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public float Growth { get; set; }
        public float Weight { get; set; }
        public PlayerPosition Position { get; set; }
        public int PlayerNumber { get; set; }
        public FootPreference WhichFoot { get; set; }
        // Footballer 1-1 Statistics
        public int StatisticsId { get; set; }
        public Statistics Statistics { get; set; }
        // Footballer 1-* IndividualTraining
        public ICollection<IndividualTraining> IndividualTrainings { get; set; } = new List<IndividualTraining>();
        // Footballer *-* GroupTraining
        public ICollection<GroupTraining> GroupTrainings { get; set; } = new List<GroupTraining>();
        // Footballer *-* Match
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
    public enum PlayerPosition
    {
        Goalkeeper,
        Defender,
        Midfielder,
        Forward,
        AttackingMidfielder,
        CentralMidfielder,
        LeftWinger,
        RightWinger
    }
    public enum FootPreference
    {
        Left,
        Right,
        Both
    }
}
    
