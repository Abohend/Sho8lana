using AutoMapper;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dto.JobProposal;

namespace src.Repository
{
	public class JobProposalRepository
	{
		private readonly Context _context;
		private readonly IMapper _mapper;

		public JobProposalRepository(Context context, IMapper mapper)
        {
			this._context = context;
			this._mapper = mapper;
		}

		public void Create(CreateJobProposalDto dto)
		{
			var proposal = _mapper.Map<JobProposal>(dto);
			_context.Add(proposal);
			_context.SaveChanges();
		}

		public ReadJobProposalDto? Read(int id)
		{
			var proposal = _context.JobsProposal.Find(id);
			return _mapper.Map<ReadJobProposalDto?>(proposal);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jobId"></param>
		/// <returns>all proposal sent to a specific job</returns>
		public List<ReadJobProposalDto>? ReadAll(int jobId)
		{
			var proposals = _context.JobsProposal.Where(p => p.JobId == jobId).ToList();
			return _mapper.Map<List<ReadJobProposalDto>?>(proposals);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="freelancerId"></param>
		/// <returns>all proposals sent to a specific freelancer</returns>
		public List<ReadJobProposalDto>? ReadAll(string freelancerId)
		{
			var proposals = _context.JobsProposal.Where(p => p.FreelancerId == freelancerId).ToList();
			return _mapper.Map<List<ReadJobProposalDto>?>(proposals);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="freelancerId"></param>
		/// <returns>pending job proposals that has been sent to specific freelancer</returns>
		public List<ReadJobProposalDto>? ReadPending(string freelancerId)
		{
			var proposals = _context.JobsProposal
				.Where(p => (p.FreelancerId == freelancerId) &&
					(p.IsAccepted == null))
				.ToList();
			return _mapper.Map<List<ReadJobProposalDto>?>(proposals);
		}

		public bool UpdateByReciever(int id, RespondJobProposalDto dto)
		{
			var proposal = _context.JobsProposal.Include(p => p.Job).SingleOrDefault(p => p.Id == id);
			if (proposal != null)
			{
				proposal.IsAccepted = dto.IsAccepted;
				proposal.Note = dto.Note;
				if (proposal.IsAccepted == true)
				{
					// update freelancer who will do the job
					proposal.Job!.FreelancerId = proposal.FreelancerId;
				}
				_context.SaveChanges();
				return true;
			}
			return false;
		}

		public bool Delete(int id)
		{
			var proposal = _context.JobsProposal.Find(id);
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
