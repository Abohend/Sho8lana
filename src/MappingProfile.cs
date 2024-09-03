using AutoMapper;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto;
using Sho8lana.Entities.Models.Dto.Category;
using Sho8lana.Entities.Models.Dto.Chat;
using Sho8lana.Entities.Models.Dto.Client;
using Sho8lana.Entities.Models.Dto.Freelancer;
using Sho8lana.Entities.Models.Dto.Job;
using Sho8lana.Entities.Models.Dto.Project;
using Sho8lana.Entities.Models.Dto.Proposal;
using Sho8lana.Entities.Models.Dto.ProposalAndReplay;

namespace Sho8lana.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterClientDto, Client>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<RegisterFreelancerDto, Freelancer>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<Client, ReadClientDto>()
                .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(des => des.ProjectsId, opt => opt.MapFrom(src => src.Projects!.Select(p => p.Id)));
            CreateMap<UpdateClientDto, Client>();

            CreateMap<Freelancer, ReadFreelancerDto>()
                .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.ImagePath));
            CreateMap<UpdateFreelancerDto, Freelancer>();

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>();

			CreateMap<Skill, SkillDto>().ReverseMap();

            CreateMap<Project, ReadProjectDto>();
            CreateMap<CreateProjectDto, Project>();

			CreateMap<ProposalReplayDto, ProposalReplay>().ReverseMap();

			CreateMap<CreateProposalDto, ProjectProposal>()
                .ForMember(des => des.ProjectId, opt => opt.MapFrom(src => src.WorkId));
            CreateMap<ProjectProposal, ReadProposalWithReplayDto>()
                .ForMember(des => des.WorkId, opt => opt.MapFrom(src => src.ProjectId));

            CreateMap<CreateProposalDto, JobProposal>()
                .ForMember(des => des.JobId, opt => opt.MapFrom(src => src.WorkId));
			CreateMap<JobProposal, ReadProposalWithReplayDto>()
			    .ForMember(des => des.WorkId, opt => opt.MapFrom(src => src.JobId));

            CreateMap<CreateJobDto, Job>();
            CreateMap<Job, ReadJobDto>();

            CreateMap<CreateGroupChatDto, GroupChat>();
            
            CreateMap<CreateMessageDto, Message>()
                .ForMember(m => m.ReceiverUserId, opt => opt.Condition(dto => dto.MessageType == MessageType.Individual))
                .ForMember(m => m.ReceiverUserId, opt => opt.MapFrom(dto => dto.ReceiverId))
                .ForMember(m => m.ReceiverGroupId, opt => opt.Condition(dto => dto.MessageType == MessageType.Group))
                .ForMember(m => m.ReceiverGroupId, opt => opt.MapFrom(dto => dto.ReceiverId));

            CreateMap<DeliveredProduct, ReadDeliveredProductDto>();
		}

	}
}
