using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Contracts
{
    public interface IRepositoryWrapper
    {
        IJobRepository Job { get; }
        IJobBidRepository JobBid { get; }
        ISiteRepository Site { get; }
        ISiteManagerRepository SiteManager { get; }
        ITruckerRepository Trucker { get; }
        IWoodTypeRepository WoodType { get; }
        IMillRepository Mill { get; }
        IMillWoodTypeRepository MillWoodType { get; }
        void Save();
    }
}
