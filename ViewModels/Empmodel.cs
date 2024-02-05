using System.ComponentModel.DataAnnotations;

namespace CustomPageApp.ViewModels
{
    public class EmpModel
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

    }
}
