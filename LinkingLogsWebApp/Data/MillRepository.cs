using LinkingLogsWebApp.Abstracts;
using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Data
{
    public class MillRepository : RepositoryBase<Mill>, IMillRepository
    {
        public MillRepository(ApplicationDbContext applicationDbContext):base(applicationDbContext)
        {

        }
    }
}
