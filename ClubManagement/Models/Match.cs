using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date {  get; set; }
        public DateTime Time { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Enemy { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public bool IsCancelledOrPostponed { get; set; }
        // Footballer *-* Match
        public ICollection<Footballer> Footballers { get; set; } = new List<Footballer>();
    }
}
