using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class GroupTraining
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; } = "Grupowy";
        public string AgeCategory { get; set; } = String.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Place { get; set; } = String.Empty;
        public DateTime DateOfTraining { get; set; }
        public DateTime StartTraining { get; set; }
        public DateTime EndTraining { get; set; }
        public TimeSpan TimeOfTraining { get; set; }

        // Footballer *-* GroupTraining
        public ICollection<Footballer> Footballers { get; set; } = new List<Footballer>();
        // Coach *-* GroupTraining
        public ICollection<Coach> Coaches { get; set; } = new List<Coach>();
    }
}
