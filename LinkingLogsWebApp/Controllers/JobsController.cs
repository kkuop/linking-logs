using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Models;
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
        // GET: Jobs
        public ActionResult Index()
        {
            return View();
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            var openJobs = _repo.Job.FindAll().Join(_repo.Site.FindAll(), a => a.SiteId, b => b.SiteId, (a, b) => new { Job = a, Site = b }).Join(_repo.SiteManager.FindAll(), a=>a.Site.SiteManagerId, b=>b.SiteManagerId, (b,c) => new { JobSite = b, SiteManager = c }).Where(a => a.SiteManager.SiteManagerId == foundUser.SiteManagerId).Select(a=>a.JobSite.Job);
            return View();
        }

        // GET: Jobs/Pending
        public ActionResult Pending()
        {
            return View();
        }

        // GET: Jobs/Pending
        public ActionResult Approved()
        {
            return View();
        }

        // GET: Jobs/Pending
        public ActionResult Completed()
        {
            return View();
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
            return View();
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
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