using System.ComponentModel.DataAnnotations;

namespace CLTTechnicalAssessmentApi.Models.Dtos
{
    public class UserDto
    {   
        public long Id { get; set; }
        public long Document { get; set; }        
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public string Birthdate { get; set; }
    }
}
