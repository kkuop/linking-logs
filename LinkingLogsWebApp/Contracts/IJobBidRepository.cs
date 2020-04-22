using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Contracts
{
    public interface IJobBidRepository : IRepositoryBase<JobBid>
    {
        public IQueryable<JobBid> ReturnWinningBids(Trucker trucker);
    }
}
