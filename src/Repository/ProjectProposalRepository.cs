using AutoMapper;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dto.ProjectProposal;

namespace src.Repository
{
	public class ProjectProposalRepository
	{
		private readonly Context _context;
		private readonly IMapper _mapper;

		public ProjectProposalRepository(Context context, IMapper mapper) 
		{
			this._context = context;
			this._mapper = mapper;
		}

		public void Create(CreateProjectProposalDto dto)
		{
			var proposal = _mapper.Map<ProjectProposal>(dto);
			_context.Add(proposal);
			_context.SaveChanges();
		}

		public ReadProjectProposal? Read(int id)
		{
			var proposal = _context.ProjectsProposal.Find(id);
			return _mapper.Map<ReadProjectProposal?>(proposal);
		}

		public List<ReadProjectProposal>? ReadAll(int projectId)
		{
			var proposals = _context.ProjectsProposal.Where(p => p.ProjectId == projectId).ToList();
			return _mapper.Map<List<ReadProjectProposal>?>(proposals);
		}

		public List<ReadProjectProposal>? ReadAll(string freelancerId)
		{
			var proposals = _context.ProjectsProposal.Where(p => p.FreelancerId == freelancerId).ToList();
			return _mapper.Map<List<ReadProjectProposal>?>(proposals);
		}

		public List<ReadProjectProposal>? ReadPending(string freelancerId)
		{
			var proposals = _context.ProjectsProposal
				.Where(p => (p.FreelancerId == freelancerId) && 
					(p.IsAccepted == null))
				.ToList();
			return _mapper.Map<List<ReadProjectProposal>?>(proposals);
		}

		public bool UpdateByClient(int id, RespondProjectProposalDto dto)
		{
			var proposal = _context.ProjectsProposal.Include(p => p.Project).SingleOrDefault(p => p.Id == id);
			if (proposal != null)
			{
				proposal.IsAccepted = dto.IsAccepted;
				proposal.ClientNote = dto.ClientNote;
				if (proposal.IsAccepted == true)
				{
					proposal.Project!.FreelancerId = proposal.FreelancerId;
				}
				_context.SaveChanges();
				return true;
			}
			return false;
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
