using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_personal.Components
{
    [Table("IDENTITIES", Schema = "users")]
    public class UserAccount
    {
        [Key]
        public Guid Id { get; set; }
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public int? State { get; set; }

        public string? Role { get; set;}
    }
}
