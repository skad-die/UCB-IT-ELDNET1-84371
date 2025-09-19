using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; 

namespace Accessio.Models
{
    public enum SemesterOption
    {
        [Display(Name = "Summer")]
        Summer,

        [Display(Name = "1st Semester")]
        FirstSemester,

        [Display(Name = "2nd Semester")]
        SecondSemester
    }

    [Index(nameof(IdNumber), IsUnique = true)]
    public class Locker
    {
        [Key]
        public int Id { get; set; }   

        [Required]
        [Display(Name = "ID Number")]
        [RegularExpression(@"^\d{5,}$", ErrorMessage = "ID Number must be at least 5 digits.")]
        public string IdNumber { get; set; } = ""; 

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        public SemesterOption Semester { get; set; } = SemesterOption.FirstSemester;

        [Required]
        [Phone]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = "";

        [Required]
        [Display(Name = "Study Load PDF")]
        public string? StudyLoadPdfPath { get; set; }
    }
}
