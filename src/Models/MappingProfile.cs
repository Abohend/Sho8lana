using AutoMapper;
using src.Models.Dto;

namespace src.Models
{
	public class MappingProfile: Profile
	{
        public MappingProfile()
        {
			CreateMap<UserRegisterDto, Freelancer>()
				.ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
			CreateMap<UserRegisterDto, Client>()
				.ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
			CreateMap<CategoryDto, Category>();
			CreateMap<Category, CategoryDto>();
		}

	}
}
