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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(u => u.IdentityUserId == userId).SingleOrDefault();
            var sites = _repo.Site.FindByCondition(s => s.SiteManagerId == foundUser.SiteManagerId).ToList();
            var updatedSites = UpdateSiteStatus(sites);
            updatedSites = updatedSites.Where(a => a.IsActive == false && a.ClosingDate < DateTime.Now).ToList();
            return View(updatedSites);
        }

        // GET: Sites/ActiveSites
        public ActionResult ActiveSites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(u => u.IdentityUserId == userId).SingleOrDefault();
            var sites = _repo.Site.FindByCondition(s => s.SiteManagerId == foundUser.SiteManagerId).ToList();
            var updatedSites = UpdateSiteStatus(sites);
            updatedSites = updatedSites.Where(a => a.IsActive == true).ToList();
            return View(updatedSites);
        }

        // GET: Sites/UpcomingSites
        public ActionResult UpcomingSites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var sites = _repo.Site.FindByCondition(s => s.SiteManagerId == foundUser.SiteManagerId).ToList();
            var updatedSites = UpdateSiteStatus(sites);
            updatedSites = updatedSites.Where(a => a.IsActive == false && a.OpeningDate > DateTime.Now).ToList();
            return View(updatedSites);
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
                return RedirectToAction("Index","SiteManagers");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sites/Edit/5
        public ActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var site = _repo.Site.FindByCondition(a => a.SiteId == id).SingleOrDefault();
            if (site.SiteManagerId == foundUser.SiteManagerId)
            {
                return View(site);
            }
            return NotFound();
        }

        // POST: Sites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Site site)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundSite = _repo.Site.FindByCondition(a => a.SiteId == site.SiteId).SingleOrDefault();
            if(foundSite != null)
            {
                if (foundUser.SiteManagerId == site.SiteManagerId)
                {
                    try
                    {
                        // TODO: Add update logic here
                        foundSite.Name = site.Name;
                        foundSite.Latitude = site.Latitude;
                        foundSite.Longitude = site.Longitude;
                        foundSite.OpeningDate = site.OpeningDate;
                        foundSite.ClosingDate = site.ClosingDate;
                        _repo.Site.Update(foundSite);
                        _repo.Save();
                        return RedirectToAction("Index","SiteManagers");
                    }
                    catch
                    {
                        return View();
                    }
                }
            }
            return View();
        }

        // GET: Sites/Delete/5
        public ActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundSite = _repo.Site.FindByCondition(a => a.SiteId == id).SingleOrDefault();
            if(foundSite.SiteManagerId == foundUser.SiteManagerId)
            {
                return View(foundSite);
            }
            return NotFound();
        }

        // POST: Sites/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Site site)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundSite = _repo.Site.FindByCondition(a => a.SiteId == id).SingleOrDefault();
            if (foundSite != null)
            {
                if (foundUser.SiteManagerId == foundSite.SiteManagerId)
                {
                    try
                    {
                        
                        _repo.Site.Delete(foundSite);
                        _repo.Save();
                        return RedirectToAction("Index", "SiteManagers");
                    }
                    catch
                    {
                        return View();
                    }
                }
            }
            return View();
        }

        public List<Site> UpdateSiteStatus(List<Site> sites)
        {
            foreach (var site in sites)
            {
                if (site.OpeningDate < DateTime.Now && site.ClosingDate > DateTime.Now)
                {
                    site.IsActive = true;
                }
                else
                {
                    site.IsActive = false;
                }
            }
            return sites;
        }
    }
}