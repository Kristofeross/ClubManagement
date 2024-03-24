using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class PrimaryMatchPlayer
    {
        [Key]
        public int Id { get; set; }
        //public int IdForEleven { get; set; }
        public int FootballerId { get; set; }
        public Footballer Footballer { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
