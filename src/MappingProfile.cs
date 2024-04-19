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

            CreateMap<Project, GetProjectDto>()
                .ForMember(des => des.CategoryDto, opt => opt.MapFrom(src => (src.Category != null)? new GetCategoryDto
				{
                    Id = src.Category.Id,
                    Name = src.Category.Name
                }: null))
                .ForMember(des => des.ClientDto, opt => opt.MapFrom(src => (src.Client != null)? new UserDto
                {
                    Id = src.ClientId,
                    Name = src.Client.Name,
                }: null));
			CreateMap<CreateProjectDto, Project>();

            CreateMap<Skill, SkillDto>();
            CreateMap<SkillDto, Skill>();
        }

    }
}
