//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using src.Data;
//using src.Models;
//using src.Models.Dto.Proposal;
//using src.Models.Dto.ProposalAndReplay;

//namespace src.Repository
//{
//	public class JobProposalRepository
//	{
//		private readonly Context _context;
//		private readonly IMapper _mapper;

//		public JobProposalRepository(Context _context, IMapper _mapper)
//		{
//			this._context = _context;
//			this._mapper = _mapper;
//		}

//		public void Create(CreateProposalDto dto)
//		{
//			var proposal = _mapper.Map<JobProposal>(dto);
//			_context.Add(proposal);
//			_context.SaveChanges();
//		}

//		public ReadProposalWithReplayDto? Read(int id)
//		{
//			var proposal = _context.JobsProposal
//				.Include(p => p.ProposalReplay)
//				.SingleOrDefault(p => p.Id == id);
//			return _mapper.Map<ReadProposalWithReplayDto?>(proposal);
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="jobId"></param>
//		/// <returns>all proposal sent to a specific job</returns>
//		public List<ReadProposalWithReplayDto>? ReadAll(int jobId)
//		{
//			var proposals = _context.JobsProposal
//				.Include(p => p.ProposalReplay)
//				.Where(p => p.JobId == jobId)
//				.ToList();
//			return _mapper.Map<List<ReadProposalWithReplayDto>?>(proposals);
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="freelancerId"></param>
//		/// <returns>all proposals sent to a specific freelancer</returns>
//		public List<ReadProposalWithReplayDto>? ReadAll(string freelancerId)
//		{
//			var proposals = _context.JobsProposal
//				.Include(p => p.ProposalReplay)
//				.Where(p => p.FreelancerId == freelancerId)
//				.ToList();
//			return _mapper.Map<List<ReadProposalWithReplayDto>?>(proposals);
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="freelancerId"></param>
//		/// <returns>pending job proposals that has been sent to specific freelancer</returns>
//		public List<ReadProposalWithReplayDto>? ReadPending(string freelancerId)
//		{
//			var proposals = _context.JobsProposal
//				.Where(p => (p.FreelancerId == freelancerId) &&
//					(p.ProposalReplay == null))
//				.ToList();
//			return _mapper.Map<List<ReadProposalWithReplayDto>?>(proposals);
//		}

//		public bool UpdateByReciever(int id, RespondJobProposalDto dto)
//		{
//			var proposal = _context.JobsProposal.Include(p => p.Job).SingleOrDefault(p => p.Id == id);
//			if (proposal != null)
//			{
//				proposal.IsAccepted = dto.IsAccepted;
//				proposal.Note = dto.Note;
//				if (proposal.IsAccepted == true)
//				{
//					// update freelancer who will do the job
//					proposal.Job!.FreelancerId = proposal.FreelancerId;
//				}
//				_context.SaveChanges();
//				return true;
//			}
//			return false;
//		}

//		public bool Delete(int id)
//		{
//			var proposal = _context.JobsProposal.Find(id);
//			if (proposal != null)
//			{
//				_context.Remove(proposal);
//				_context.SaveChanges();
//				return true;
//			}
//			return false;
//		}
//	}
//}
