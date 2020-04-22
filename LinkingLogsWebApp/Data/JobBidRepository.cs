using LinkingLogsWebApp.Abstracts;
using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Data
{
    public class JobBidRepository : RepositoryBase<JobBid>, IJobBidRepository
    {
        public JobBidRepository(ApplicationDbContext applicationDbContext):base(applicationDbContext)
        {

        }
        public IQueryable<JobBid> ReturnWinningBids(Trucker trucker) => FindByCondition(a => a.TruckerId == trucker.TruckerId && a.IsWinningBid == true);
    }
}
