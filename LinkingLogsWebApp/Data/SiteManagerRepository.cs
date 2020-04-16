using LinkingLogsWebApp.Abstracts;
using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Data
{
    public class SiteManagerRepository : RepositoryBase<SiteManager>, ISiteManagerRepository
    {
        public SiteManagerRepository(ApplicationDbContext applicationDbContext):base(applicationDbContext)
        {

        }
    }
}
