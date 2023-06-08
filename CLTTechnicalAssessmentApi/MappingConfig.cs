using AutoMapper;
using CLTTechnicalAssessmentApi.Models;
using CLTTechnicalAssessmentApi.Models.Dtos;

namespace CLTTechnicalAssessmentApi
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();


            CreateMap<DateOnly, string>()
                .ConvertUsing(input => input.ToString());
            CreateMap<string, DateOnly>()
                .ConvertUsing(input => DateOnly.Parse(input));

        }
    }
}
