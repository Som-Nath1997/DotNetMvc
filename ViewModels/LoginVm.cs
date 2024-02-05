using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace CustomPageApp.ViewModels
{
    public class LoginVm
    {
        [Key]
        [Required(ErrorMessage = "UserName is Required")]
        public string  UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]

        [DataType(DataType.Password)]
        public string   Password { get; set; }

        [Display(Name = "RememberMe")]
        public  bool  RememberMe { get; set; }
    }
}
    