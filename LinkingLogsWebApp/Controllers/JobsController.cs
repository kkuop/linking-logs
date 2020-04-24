using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Models;
using LinkingLogsWebApp.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkingLogsWebApp.Controllers
{
    [Authorize(Roles ="SiteManager,Trucker")]
    public class JobsController : Controller
    {
        private IRepositoryWrapper _repo;
        public JobsController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var sites = _repo.Site.FindByCondition(a => a.SiteManagerId == foundUser.SiteManagerId).Where(a => a.IsActive == true).OrderBy(a=>a.Name);
            var woodTypes = _repo.WoodType.FindAll().OrderBy(a=>a.Type);
            var mills = _repo.Mill.FindAll().OrderBy(a => a.Name);
            Job job = new Job()
            {
                Sites = sites,
                WoodTypes = woodTypes,
                Mills = mills
            };
            return View(job);
        }

        // GET: Jobs/Open
        public ActionResult Open()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var openJobs = _repo.Job.FindAll().Join(_repo.Site.FindAll(), a => a.SiteId, b => b.SiteId, (a, b) => new { Job = a, Site = b }).Join(_repo.SiteManager.FindAll(), a=>a.Site.SiteManagerId, b=>b.SiteManagerId, (b,c) => new { JobSite = b, SiteManager = c }).Where(a => a.SiteManager.SiteManagerId == foundUser.SiteManagerId);
            OpenJobsViewModel jobSite = new OpenJobsViewModel()
            {
                Jobs = openJobs.Select(a => a.JobSite.Job).Where(a => a.Status == "Open").ToList(),
                Sites = openJobs.Select(a => a.JobSite.Site).ToList(),
                SiteManager = openJobs.Select(a => a.SiteManager).FirstOrDefault()
            };
            foreach(var job in jobSite.Jobs)
            {
                var foundSite = _repo.Site.FindByCondition(a => a.SiteId == job.SiteId).SingleOrDefault();
                var foundWood = _repo.WoodType.FindByCondition(a => a.WoodTypeId == job.WoodTypeId).SingleOrDefault();
                var foundMill = _repo.Mill.FindByCondition(a => a.MillId == job.MillId).SingleOrDefault();
                job.Site = foundSite;
                job.WoodType = foundWood;
                job.Mill = foundMill;
            }
            return View(jobSite);
        }

        // GET: Jobs/Pending
        public ActionResult Pending()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var openJobs = _repo.Job.FindAll().Join(_repo.Site.FindAll(), a => a.SiteId, b => b.SiteId, (a, b) => new { Job = a, Site = b }).Join(_repo.SiteManager.FindAll(), a => a.Site.SiteManagerId, b => b.SiteManagerId, (b, c) => new { JobSite = b, SiteManager = c }).Where(a => a.SiteManager.SiteManagerId == foundUser.SiteManagerId);
            OpenJobsViewModel jobSite = new OpenJobsViewModel()
            {
                Jobs = openJobs.Select(a => a.JobSite.Job).Where(a => a.Status == "Pending").ToList(),
                Sites = openJobs.Select(a => a.JobSite.Site).ToList(),
                SiteManager = openJobs.Select(a => a.SiteManager).FirstOrDefault(),
                JobBids = _repo.JobBid.FindAll()
            };
            foreach (var job in jobSite.Jobs)
            {
                var foundSite = _repo.Site.FindByCondition(a => a.SiteId == job.SiteId).SingleOrDefault();
                var foundWood = _repo.WoodType.FindByCondition(a => a.WoodTypeId == job.WoodTypeId).SingleOrDefault();
                job.Site = foundSite;
                job.WoodType = foundWood;
            }
            return View(jobSite);
        }

        // GET: Jobs/Pending
        public ActionResult Approved()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var openJobs = _repo.Job.FindAll().Join(_repo.Site.FindAll(), a => a.SiteId, b => b.SiteId, (a, b) => new { Job = a, Site = b }).Join(_repo.SiteManager.FindAll(), a => a.Site.SiteManagerId, b => b.SiteManagerId, (b, c) => new { JobSite = b, SiteManager = c }).Where(a => a.SiteManager.SiteManagerId == foundUser.SiteManagerId);
            OpenJobsViewModel jobSite = new OpenJobsViewModel()
            {
                Jobs = openJobs.Select(a => a.JobSite.Job).Where(a => a.Status == "Approved").ToList(),
                Sites = openJobs.Select(a => a.JobSite.Site).ToList(),
                SiteManager = openJobs.Select(a => a.SiteManager).FirstOrDefault()
            };
            foreach (var job in jobSite.Jobs)
            {
                var foundSite = _repo.Site.FindByCondition(a => a.SiteId == job.SiteId).SingleOrDefault();
                var foundWood = _repo.WoodType.FindByCondition(a => a.WoodTypeId == job.WoodTypeId).SingleOrDefault();
                var foundMill = _repo.Mill.FindByCondition(a => a.MillId == job.MillId).SingleOrDefault();
                job.Site = foundSite;
                job.WoodType = foundWood;
                job.Mill = foundMill;
            }
            return View(jobSite);
        }

        // GET: Jobs/Pending
        public ActionResult Completed()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var openJobs = _repo.Job.FindAll().Join(_repo.Site.FindAll(), a => a.SiteId, b => b.SiteId, (a, b) => new { Job = a, Site = b }).Join(_repo.SiteManager.FindAll(), a => a.Site.SiteManagerId, b => b.SiteManagerId, (b, c) => new { JobSite = b, SiteManager = c }).Where(a => a.SiteManager.SiteManagerId == foundUser.SiteManagerId);
            OpenJobsViewModel jobSite = new OpenJobsViewModel()
            {
                Jobs = openJobs.Select(a => a.JobSite.Job).Where(a => a.Status == "Completed").ToList(),
                Sites = openJobs.Select(a => a.JobSite.Site).ToList(),
                SiteManager = openJobs.Select(a => a.SiteManager).FirstOrDefault()
            };
            foreach (var job in jobSite.Jobs)
            {
                var foundSite = _repo.Site.FindByCondition(a => a.SiteId == job.SiteId).SingleOrDefault();
                var foundWood = _repo.WoodType.FindByCondition(a => a.WoodTypeId == job.WoodTypeId).SingleOrDefault();
                job.Site = foundSite;
                job.WoodType = foundWood;
            }
            return View(jobSite);
        }

        // GET: Jobs/ViewBids
        public ActionResult ViewBids(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var jobBids = _repo.JobBid.FindByCondition(a => a.JobId == id).ToList();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            var allTruckers = _repo.Trucker.FindAll().ToList();
            foreach(var item in allTruckers)
            {
                foreach (var bid in jobBids)
                {
                    if (bid.TruckerId == item.TruckerId)
                    {
                        bid.Trucker = item;
                    }
                }
            }
            ViewBidModel model = new ViewBidModel()
            {
                Job = foundJob,
                JobBids = jobBids,
                
            };
            return View(model);
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            try
            {
                // TODO: Add insert logic here
                for(var i = 0; i < job.Loads; i++)
                {
                    if(i > 0)
                    {
                        job.JobId = 0;
                    }
                    job.Status = "Open";
                    _repo.Job.Create(job);
                    _repo.Save();
                }
                return RedirectToAction("Index","SiteManagers");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            var foundSite = _repo.Site.FindByCondition(a => a.SiteId == foundJob.SiteId).SingleOrDefault();
            var sites = _repo.Site.FindByCondition(a => a.SiteManagerId == foundUser.SiteManagerId).OrderBy(a => a.Name);
            var woodTypes = _repo.WoodType.FindAll().OrderBy(a => a.Type);
            foundJob.Site = foundSite;
            foundJob.Sites = sites;
            foundJob.WoodTypes = woodTypes;
            if(foundUser.SiteManagerId == foundJob.Site.SiteManagerId)
            {
                return View(foundJob);
            }
            return NotFound();
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Job job)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            try
            {
                // TODO: Add update logic here
                foundJob.LoadSize = job.LoadSize;
                foundJob.SiteId = job.SiteId;
                foundJob.WoodTypeId = job.WoodTypeId;
                _repo.Job.Update(foundJob);
                _repo.Save();
                return RedirectToAction("Index","SiteManagers");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobs/Deliver/5
        public ActionResult Deliver(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            foundJob.Mill = _repo.Mill.FindByCondition(a => a.MillId == foundJob.MillId).SingleOrDefault();
            foundJob.Site = _repo.Site.FindByCondition(a => a.SiteId == foundJob.SiteId).SingleOrDefault();
            foundJob.WoodType = _repo.WoodType.FindByCondition(a => a.WoodTypeId == foundJob.WoodTypeId).SingleOrDefault();
            return View(foundJob);
        }

        // POST: Jobs/Deliver
        [HttpPost]
        public ActionResult Deliver(int id, Job job)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            foundJob.Status = "Completed";
            _repo.Job.Update(foundJob);
            _repo.Save();
            return RedirectToAction("Index", "Truckers");
        }



        // GET: Jobs/Delete/5
        public ActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            return View(foundJob);
        }

        // POST: Jobs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Job job)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            try
            {
                // TODO: Add delete logic here
                var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
                _repo.Job.Delete(foundJob);
                _repo.Save();
                return RedirectToAction("Index","SiteManagers");
            }
            catch
            {
                return View();
            }
        }
    }
}