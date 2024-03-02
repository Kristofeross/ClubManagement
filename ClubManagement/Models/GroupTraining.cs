using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class GroupTraining
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public AgeCategoriesCopy AgeCategory { get; set; }
        public DateTime DateOfTraining { get; set; }
        public DateTime StartTraining { get; set; }
        public DateTime EndTraining { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Footballer *-* GroupTraining
        public ICollection<Footballer> Footballers { get; set; } = new List<Footballer>();
    }
}
