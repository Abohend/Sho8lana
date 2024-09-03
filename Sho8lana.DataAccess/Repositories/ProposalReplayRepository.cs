using AutoMapper;
using Sho8lana.DataAccess.Data;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.ProposalAndReplay;

namespace Sho8lana.DataAccess.Repositories
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
