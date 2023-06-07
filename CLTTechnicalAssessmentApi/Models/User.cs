using System.ComponentModel.DataAnnotations;

namespace CLTTechnicalAssessmentApi.Models
{
    public class User
    {
        [Key]
        public long Document { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(30)]
        public string Phone { get; set; }
        [Required]
        public DateOnly Birthdate { get; set; }


    }
}
