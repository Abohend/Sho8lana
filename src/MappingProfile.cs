using AutoMapper;
using src.Models;
using src.Models.Dto;
using src.Models.Dto.Category;
using src.Models.Dto.Project;

namespace src
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterClientDto, Client>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<RegisterFreelancerDto, Freelancer>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>();

			CreateMap<Skill, SkillDto>().ReverseMap();

			CreateMap<Client, UserDto>();
			CreateMap<Project, GetProjectDto>()
				.ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
				.ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills));
            CreateMap<CreateProjectDto, Project>();

        }

    }
}
