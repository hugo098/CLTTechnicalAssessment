using System.ComponentModel.DataAnnotations;

namespace CLTTechnicalAssessmentApi.Models.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long Document { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        [EmailAddress]
        public string Email { get; set; }
        /// <example>+595971399987</example>
        [Required]
        [MaxLength(30)]
        [MinLength(1)]
        [Phone]
        [RegularExpression(@"^[\+][0-9]{12}$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string Birthdate { get; set; }
    }
}
