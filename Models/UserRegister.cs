using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ranga.Models
{
     public class User{
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Column("email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [Column("password")]
        public string Password { get; set; }
    }
    [Table("users")]
    public class UserRegister : User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your first name")]
        [Column("first_name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your last name")]
        [Column("last_name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your address")]
        [Column("address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Column("confirm_password")] 
        public string ConfirmPassword { get; set; }
    }
}
