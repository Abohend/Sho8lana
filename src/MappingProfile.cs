using AutoMapper;
using src.Models;
using src.Models.Dto;

namespace src
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDto, Freelancer>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<UserRegisterDto, Client>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Job, GetJobDto>()
                .ForMember(des => des.CategoryDto, opt => opt.MapFrom(src => (src.Category != null)? new CategoryDto
                {
                    Id = src.CategoryId,
                    Name = src.Category.Name
                }: null))
                .ForMember(des => des.ClientDto, opt => opt.MapFrom(src => (src.Client != null)? new UserDto
                {
                    Id = src.ClientId,
                    Name = src.Client.Name,
                }: null));
			CreateMap<CreateJobDto, Job>();
                
        }

    }
}
