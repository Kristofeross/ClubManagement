using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClubManagement.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public RoleType Name { get; set; } = string.Empty;
        // Do Account
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }

    // Na typie enum
    public enum RoleType
    {
        Coach,
        Player,
        Admin
    }
}
