using System.ComponentModel.DataAnnotations;

namespace Accessio.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Lastname { get; set; } = "";

        [Required, MaxLength(25)]
        public string Firstname { get; set; } = "";

        [MaxLength(50)]
        public string? Course { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
