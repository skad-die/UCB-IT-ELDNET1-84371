using System.ComponentModel.DataAnnotations;

namespace Accessio.Models
{
    public enum RoleOption
    {
        [Display(Name = "Faculty")]
        Faculty,

        [Display(Name = "Non-Teaching Staff")]
        NonTeachingStaff
    }
    public enum DepartmentOption
    {
        [Display(Name = "College of Law")]
        CollegeOfLaw,

        [Display(Name = "College of Computer Studies")]
        CollegeOfCs,

        [Display(Name = "College of Engineering")]
        CollegeOfEngineering,

        [Display(Name = "College of Business")]
        CollegeOfBusiness,

        [Display(Name = "College of Education")]
        CollegeOfEducation,

        [Display(Name = "College of Arts and Sciences")]
        CollegeOfArtsAndSciences,

        [Display(Name = "College of Nursing")]
        CollegeOfNursing,

        [Display(Name = "Senior High School")]
        SeniorHighSchool
    }

    public class GatePass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; } = "";

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; }

        [Required]
        [Display(Name = "Role")]
        public RoleOption Role { get; set; }

        [Required]
        [Display(Name = "Department")]
        public DepartmentOption Department { get; set; }

        [Display(Name = "Course & Year")]
        public string? CourseAndYear { get; set; }

        [Display(Name = "Vehicle Plate No.")]
        public string? VehiclePlateNo { get; set; }

        [Display(Name = "Registration Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? RegistrationExpiryDate { get; set; }

        [Display(Name = "Vehicle Type")]
        public string? VehicleType { get; set; }

        [Display(Name = "Maker")]
        public string? Maker { get; set; }

        [Display(Name = "Color")]
        public string? Color { get; set; }

        [Required]
        [Display(Name = "Study Load PDF")]
        public string? StudyLoadPdfPath { get; set; }

        [Required]
        [Display(Name = "Vehicle Registration PDF")]
        public string? RegistrationPdfPath { get; set; }

    }
}
