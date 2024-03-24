using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        public int? MainCoachId { get; set; }
        public string Enemy { get; set; } = string.Empty;
        public string? Place { get; set; }
        public string AgeCategory { get; set; } = string.Empty;
        public DateTime? DateOfMatch {  get; set; }
        public DateTime? StartMatch { get; set; }
        public string? Score { get; set; }
        public string? ScoreStatus { get; set; }
        public string MatchHost { get; set; } = string.Empty;
        public string MatchStatus {  get; set; } = string.Empty;
        public string TypeOfMatch {  get; set; } = string.Empty;

        // Footballer *-* Match
        public ICollection<Footballer> Footballers { get; set; } = new List<Footballer>();

        // Relacja jeden do jeden z StartingEleven
        public ICollection<PrimaryMatchPlayer> PrimaryMatchPlayers { get; set; } = new List<PrimaryMatchPlayer>();
        public ICollection<SubstituteMatchPlayer> SubstituteMatchPlayers { get; set; } = new List<SubstituteMatchPlayer>();

        // Coach 1-* Match
        public Coach? MainCoach { get; set; }
    }
}
