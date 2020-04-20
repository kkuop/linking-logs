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
            var sites = _repo.Site.FindByCondition(a => a.SiteManagerId == foundUser.SiteManagerId).OrderBy(a=>a.Name);
            var woodTypes = _repo.WoodType.FindAll().OrderBy(a=>a.Type);
            Job job = new Job()
            {
                Sites = sites,
                WoodTypes = woodTypes
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
                job.Site = foundSite;
                job.WoodType = foundWood;
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
                job.Site = foundSite;
                job.WoodType = foundWood;
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

        // GET: Jobs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Jobs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}