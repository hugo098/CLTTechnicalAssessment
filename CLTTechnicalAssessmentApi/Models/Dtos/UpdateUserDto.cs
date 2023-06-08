using System.ComponentModel.DataAnnotations;

namespace CLTTechnicalAssessmentApi.Models.Dtos
{
    public class UpdateUserDto
    {
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
        [RegularExpression(@"\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])*", ErrorMessage = "Date incorrect format. Format must match yyyy-mm-dd")]        
        public string Birthdate { get; set; }
    }
}
