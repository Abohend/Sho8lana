using AutoMapper;
using src.Data;
using src.Models;
using src.Models.Dto.ProposalAndReplay;

namespace src.Repository
{
	public class ProposalReplayRepository
	{
		private readonly Context _context;
		private readonly IMapper _mapper;

		public ProposalReplayRepository(Context context, IMapper mapper)
        {
			this._context = context;
			this._mapper = mapper;
		}

		public void Create(ProposalReplayDto replayDto, int proposalId)
		{
			var replay = _mapper.Map<ProposalReplay>(replayDto);
			replay.ProposalId = proposalId;
			_context.Add(replay);
			_context.SaveChanges();
		}
    }
}
