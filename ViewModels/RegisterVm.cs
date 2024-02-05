using System.ComponentModel.DataAnnotations;

namespace CustomPageApp.ViewModels
{
    public class RegisterVm
    {
        [Key]
        public int Id { get; set; } 
        [Required(ErrorMessage = "Name is required.")]
        public string ? Name { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string ? Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ? ConfirmPassword { get; set; }



        [Required(ErrorMessage = "Address is required.")]
        public string ? Address { get; set; }
    }
}   
