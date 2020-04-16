﻿using LinkingLogsWebApp.Contracts;
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
