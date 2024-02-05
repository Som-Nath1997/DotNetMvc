using System.ComponentModel.DataAnnotations;

namespace CustomPageApp.ViewModels
{
    public class StudentVMm
    {
        [Required]
        public string ? Name  { get; set; }
        [Required]
        public string ? Email {get; set; }
        public string  ? Phone { get;  set; }
    }
}
