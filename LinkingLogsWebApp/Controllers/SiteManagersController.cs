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
    [Authorize(Roles="SiteManager")]
    public class SiteManagersController : Controller
    {
        private IRepositoryWrapper _repo;
        public SiteManagersController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        // GET: SiteManagers
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            if(foundUser == null)
            {
                return RedirectToAction("Create");
            }
            var jobs = _repo.Job.FindAll().Join(_repo.Site.FindAll(), a => a.SiteId, b => b.SiteId, (a, b) => new { Job = a, Site = b }).Join(_repo.SiteManager.FindAll(), a => a.Site.SiteManagerId, b => b.SiteManagerId, (b, c) => new { JobSite = b, SiteManager = c }).Where(a => a.SiteManager.SiteManagerId == foundUser.SiteManagerId);
            SiteManagerIndexViewModel siteManagerIndexViewModel = new SiteManagerIndexViewModel()
            {
                AllSites = _repo.Site.FindByCondition(a => a.SiteManagerId == foundUser.SiteManagerId),
                UpcomingSites = _repo.Site.FindByCondition(a => a.OpeningDate > DateTime.Now),
                ActiveSites = _repo.Site.FindByCondition(a => a.OpeningDate < DateTime.Now && a.ClosingDate > DateTime.Now && a.SiteManagerId == foundUser.SiteManagerId),
                OpenJobs = jobs.Select(a => a.JobSite.Job).Where(a => a.Status == "Open"),
                PendingJobs = jobs.Select(a => a.JobSite.Job).Where(a => a.Status == "Pending"),
                ApprovedJobs = jobs.Select(a => a.JobSite.Job).Where(a => a.Status == "Approved")
            };
            return View(siteManagerIndexViewModel);
        }

        // GET: SiteManagers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SiteManagers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiteManagers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteManager siteManager)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                // TODO: Add insert logic here
                siteManager.IdentityUserId = userId;
                _repo.SiteManager.Create(siteManager);
                _repo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SiteManagers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SiteManagers/Edit/5
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

        // GET: SiteManagers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SiteManagers/Delete/5
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