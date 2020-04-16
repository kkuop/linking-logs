using LinkingLogsWebApp.Abstracts;
using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Data
{
    public class TruckerRepository : RepositoryBase<Trucker>, ITruckerRepository
    {
        public TruckerRepository(ApplicationDbContext applicationDbContext):base(applicationDbContext)
        {

        }
    }
}
