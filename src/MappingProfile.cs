﻿using AutoMapper;
using src.Models;
using src.Models.Dto;
using src.Models.Dto.Category;
using src.Models.Dto.Client;
using src.Models.Dto.Freelancer;
using src.Models.Dto.Job;
using src.Models.Dto.Project;
using src.Models.Dto.Proposal;
using src.Models.Dto.ProposalAndReplay;

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

            CreateMap<Client, GetClientDto>()
                .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(des => des.ProjectsId, opt => opt.MapFrom(src => src.Projects!.Select(p => p.Id)));
            CreateMap<UpdateClientDto, Client>();

            CreateMap<Freelancer, GetFreelancerDto>()
                .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.ImagePath));
            CreateMap<UpdateFreelancerDto, Freelancer>();

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>();

			CreateMap<Skill, SkillDto>().ReverseMap();

			CreateMap<Client, UserDto>();
            CreateMap<Project, GetProjectDto>();
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
        }

    }
}
