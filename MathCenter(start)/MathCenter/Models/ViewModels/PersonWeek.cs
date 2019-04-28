using System.ComponentModel.DataAnnotations;

namespace MathCenter.Models.ViewModels
{
    public class PersonWeek
    {
        [Key]
        [StringLength(8)]
        public string VNum { get; set; }

        [Required]
        public int Week { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}