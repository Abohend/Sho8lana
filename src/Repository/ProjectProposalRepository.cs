using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using src.Data;
using src.Models;
using src.Models.Dto.Proposal;
using src.Models.Dto.ProposalAndReplay;

namespace src.Repository
{
	public class ProjectProposalRepository
	{
		private readonly Context _context;
		private readonly IMapper _mapper;
		private readonly ProposalReplayRepository _proposalReplayRepo;

		public ProjectProposalRepository(Context context, IMapper mapper
			, ProposalReplayRepository proposalReplayRepository)
		{
			this._context = context;
			this._mapper = mapper;
			this._proposalReplayRepo = proposalReplayRepository;
		}

		public void Create(CreateProposalDto dto)
		{
			var proposal = _mapper.Map<ProjectProposal>(dto);
			_context.Add(proposal);
			_context.SaveChanges();
		}

		public ReadProposalWithReplayDto? Read(int id)
		{
			var proposal = _context.ProjectsProposal.Include(p => p.ProposalReplay).SingleOrDefault(p => p.Id == id);
			return _mapper.Map<ReadProposalWithReplayDto?>(proposal);
		}

		public List<ReadProposalWithReplayDto>? ReadAll(int projectId)
		{
			var proposals = _context.ProjectsProposal.Include(p => p.ProposalReplay).Where(p => p.ProjectId == projectId).ToList();
			return _mapper.Map<List<ReadProposalWithReplayDto>?>(proposals);
		}

		public List<ReadProposalWithReplayDto>? ReadAll(string freelancerId)
		{
			var proposals = _context.ProjectsProposal.Include(p => p.ProposalReplay).Where(p => p.FreelancerId == freelancerId).ToList();
			return _mapper.Map<List<ReadProposalWithReplayDto>?>(proposals);
		}

		public List<ReadProposalWithReplayDto>? ReadAccepted(string freelancerId)
		{
			return ReadAll(freelancerId)?
					.Where(pr => pr.ProposalReplay != null)
					.Where(pr => pr.ProposalReplay!.IsAccepted == true)
					.ToList();
		}

		public ReadProposalWithReplayDto? ReadAccepted(int projectId)
		{
			return ReadAll(projectId)?
					.Where(pr => pr.ProposalReplay != null)
					.SingleOrDefault(pr => pr.ProposalReplay!.IsAccepted == true);
		}

		public List<ReadProposalWithReplayDto>? ReadPending(string freelancerId)
		{
			var proposals = _context.ProjectsProposal
				.Where(p => (p.FreelancerId == freelancerId) &&
					(p.ProposalReplay == null))
				.ToList();
			return _mapper.Map<List<ReadProposalWithReplayDto>?>(proposals);
		}

		public bool Delete(int id)
		{
			var proposal = _context.ProjectsProposal.Find(id);
			if (proposal != null)
			{
				_context.Remove(proposal);
				_context.SaveChanges();
				return true;
			}
			return false;
		}
	}
}
