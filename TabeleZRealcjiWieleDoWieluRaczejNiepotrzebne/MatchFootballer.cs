namespace ClubManagement.Models
{
    public class MatchFootballer
    {
        public int FootballerId { get; set; }
        public Footballer Footballer { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
