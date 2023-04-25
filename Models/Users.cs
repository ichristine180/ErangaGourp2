using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ranga.Models
{
    public class Users
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("names")]
        public string Names { get; set; }
        [Required]
        [Column("email")] public string Email { get; set; }
        [Required]
        [Column("password")] public string Password { get; set; }


    }
}

