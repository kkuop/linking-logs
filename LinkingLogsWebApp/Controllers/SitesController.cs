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
    [Authorize(Roles="SiteManager")]
    public class SitesController : Controller
    {
        private IRepositoryWrapper _repo;
        public SitesController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        // GET: Sites
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sites/ClosedSites
        public ActionResult ClosedSites()
        {
            return View();
        }

        // GET: Sites/ActiveSites
        public ActionResult ActiveSites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(u => u.IdentityUserId == userId).SingleOrDefault();
            var sites = _repo.Site.FindByCondition(s => s.SiteManagerId == foundUser.SiteManagerId);
            foreach(var site in sites)
            {
                if(site.OpeningDate < DateTime.Now && site.ClosingDate > DateTime.Now)
                {
                    site.IsActive = true;
                }
                else
                {
                    site.IsActive = false;
                }
            }
            sites = sites.Where(a => a.IsActive == true);
            return View(sites);
        }

        // GET: Sites/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Site site)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            try
            {
                // TODO: Add insert logic here
                if(site.OpeningDate < DateTime.Now && site.ClosingDate > DateTime.Now)
                {
                    site.IsActive = true;
                }
                site.SiteManagerId = foundUser.SiteManagerId;
                _repo.Site.Create(site);
                _repo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sites/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sites/Edit/5
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

        // GET: Sites/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sites/Delete/5
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