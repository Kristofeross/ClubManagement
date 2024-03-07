using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManagement.Models
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }
        public int Match { get; set; }
        public int Minutes { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set;}
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        // Footballer - Statistics | One to one
        public int? FootballerId { get; set; }
        public Footballer Footballer { get; set; }
        //public Footballer Footballer { get; set; } = null!;
    }
}
