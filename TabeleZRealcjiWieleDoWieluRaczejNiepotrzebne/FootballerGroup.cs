namespace ClubManagement.Models
{
    public class FootballerGroup
    {
        public int FootballerId { get; set; }
        public Footballer Footballer { get; set; }

        public int GroupTrainingId { get; set; }
        public GroupTraining GroupTraining { get; set; }
    }
}
