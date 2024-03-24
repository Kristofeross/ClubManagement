using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class SubstituteMatchPlayer
    {
        [Key]
        public int Id { get; set; }
        public int IdForSubstitute { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
