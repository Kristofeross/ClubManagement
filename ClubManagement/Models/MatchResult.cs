namespace ClubManagement.Models
{
    public class MatchResult
    {
        public int Id { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }

        public MatchResult(int homeTeamScore, int awayTeamScore)
        {
            HomeTeamScore = homeTeamScore;
            AwayTeamScore = awayTeamScore;
        }

        public override string ToString()
        {
            return $"{HomeTeamScore}-{AwayTeamScore}";
        }
    }
}
