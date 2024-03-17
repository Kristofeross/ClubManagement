using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class IndividualTraining
    {
        [Key]
        public int Id { get; set; }
        public int FootballerId { get; set; }
        public int? CoachId { get; set; }
        public string Type { get; set; } = "Indywidualny";
        public DateTime DateOfTraining { get; set; }
        public DateTime StartTraining { get; set; }
        public DateTime EndTraining { get; set; }
        public TimeSpan TimeOfTraining { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Place {  get; set; }
        //public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

        // Footballer 1-* IndividualTraining 
        public Footballer Footballer { get; set; }
        // Coach 1-* IndividualTraining 
        public Coach? Coach { get; set; }
    }
}
