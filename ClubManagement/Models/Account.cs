using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManagement.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /*public string AccountName { get; set; } = string.Empty;*/
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        // Do roli
        //public Role Role { get; set; }
        //public int RoleId { get; set; }
        //public virtual Role Role { get; set; } // Kiedy miałem dto
        /* Kiedy korzystałem z osobnej klasy do roli */


    }
}
