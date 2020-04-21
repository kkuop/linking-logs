using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _context;
        private IJobRepository _job;
        private IJobBidRepository _jobBid;
        private ISiteRepository _site;
        private ISiteManagerRepository _siteManager;
        private ITruckerRepository _trucker;
        private IWoodTypeRepository _woodType;
        private IMillRepository _mill;
        private IMillWoodTypeRepository _millWoodType;
        public IJobRepository Job
        {
            get
            {
                if(_job == null)
                {
                    _job = new JobRepository(_context);
                }
                return _job;
            }
        }
        public IJobBidRepository JobBid
        {
            get
            {
                if(_jobBid == null)
                {
                    _jobBid = new JobBidRepository(_context);
                }
                return _jobBid;
            }
        }
        public ISiteRepository Site
        {
            get
            {
                if(_site == null)
                {
                    _site = new SiteRepository(_context);
                }
                return _site;
            }
        }
        public ISiteManagerRepository SiteManager
        {
            get
            {
                if(_siteManager == null)
                {
                    _siteManager = new SiteManagerRepository(_context);
                }
                return _siteManager;
            }
        }
        public ITruckerRepository Trucker { 
            get
            {
                if(_trucker == null)
                {
                    _trucker = new TruckerRepository(_context);
                }
                return _trucker;
            } 
        }
        public IWoodTypeRepository WoodType
        {
            get
            {
                if(_woodType == null)
                {
                    _woodType = new WoodTypeRepository(_context);
                }
                return _woodType;
            }
        }
        public IMillRepository Mill
        {
            get
            {
                if(_mill == null)
                {
                    _mill = new MillRepository(_context);
                }
                return _mill;
            }
        }
        public IMillWoodTypeRepository MillWoodType
        {
            get
            {
                if (_millWoodType == null)
                {
                    _millWoodType = new MillWoodTypeRepository(_context);
                }
                return _millWoodType;
            }
        }
        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
