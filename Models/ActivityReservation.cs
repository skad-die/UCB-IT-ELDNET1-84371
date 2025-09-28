using System.ComponentModel.DataAnnotations;

namespace Accessio.Models
{
    public enum OrganizationOption
    {
        [Display(Name = "College of Law")]
        CollegeOfLaw,

        [Display(Name = "College of Computer Studies")]
        CollegeOfCS,

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

    public class ActivityReservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department")]
        public OrganizationOption Department { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; }

        [Required]
        [Display(Name = "Activity Title")]
        public string ActivityTitle { get; set; } = "";

        [Required]
        [Display(Name = "Speaker")]
        public string SpeakerName { get; set; } = "";

        [Required]
        [Display(Name = "Venue")]
        public string Venue { get; set; } = "";

        [Required]
        [Display(Name = "Purpose Objective")]
        public string PurposeObjective { get; set; } = "";

        [Required]
        [Display(Name = "Equipment & Others Facilities Needed")]
        public string EventEquipmentRequest { get; set; } = "";

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Needed")]
        public DateTime DateNeeded { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Time From")]
        public TimeSpan TimeFrom { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Time To")]
        public TimeSpan TimeTo { get; set; }

        [Required]
        [Display(Name = "Participants")]
        public string Participants { get; set; } = "";

        [Required]
        [Display(Name = "Nature of Activity")]
        public string NatureOfActivity { get; set; } = "";

        [Display(Name = "Source of Funds")]
        public string? SourceOfFunds { get; set; }
    }
}
