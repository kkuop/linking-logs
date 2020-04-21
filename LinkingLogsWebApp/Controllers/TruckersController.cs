using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LinkingLogsWebApp.Contracts;
using LinkingLogsWebApp.Models;
using LinkingLogsWebApp.Services;
using LinkingLogsWebApp.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkingLogsWebApp.Controllers
{
    [Authorize(Roles = "Trucker")]
    public class TruckersController : Controller
    {
        private IRepositoryWrapper _repo;
        private GoogleGeocodeService _geocode;
        private GoogleDistanceService _distance;
        public TruckersController(IRepositoryWrapper repo, GoogleGeocodeService geocode, GoogleDistanceService distance)
        {
            _repo = repo;
            _geocode = geocode;
            _distance = distance;
        }
        // GET: Truckers
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            if (foundUser == null)
            {
                return RedirectToAction("Create");
            }
            //get a list of open jobs and check the distance
            var jobSite = _repo.Job.FindAll().Join(_repo.Site.FindAll(), a => a.SiteId, b => b.SiteId, (a, b) => new { Job = a, Site = b });
            var jobs = jobSite.Select(a => a.Job).Where(a => a.Status == "Open").ToList();
            var pendingJobs = jobSite.Select(a => a.Job).Where(a => a.Status == "Pending").ToList();
            foreach(var job in jobs)
            {
                job.Site = _repo.Site.FindByCondition(a => a.SiteId == job.SiteId).SingleOrDefault();
                job.Mill = _repo.Mill.FindByCondition(a => a.MillId == job.MillId).SingleOrDefault();
                job.WoodType = _repo.WoodType.FindByCondition(a => a.WoodTypeId == job.WoodTypeId).SingleOrDefault();
                var result = await _distance.GetDistance(job, foundUser, "HomeToSite");
                var distance = result.rows[0].elements[0].distance.text;
                distance = distance.Replace("m","").Replace("i","").Trim();
                var dd = double.Parse(distance);
                job.Distance = dd;
            }
            TruckerIndexViewModel model = new TruckerIndexViewModel()
            {
                SuggestedJobs = jobs,
                PendingJobs = pendingJobs
            };
            return View(model);
        }

        // GET: Truckers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Truckers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Truckers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Trucker trucker)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try
            {
                // TODO: Add insert logic here
                var coords = _geocode.GetCoords(trucker);
                trucker.Latitude = coords.Result.results[0].geometry.location.lat;
                trucker.Longitude = coords.Result.results[0].geometry.location.lng;
                trucker.IdentityUserId = userId;
                _repo.Trucker.Create(trucker);
                _repo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Truckers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Truckers/Edit/5
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

        // GET: Truckers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Truckers/Delete/5
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