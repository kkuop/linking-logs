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
using static LinkingLogsWebApp.Services.GoogleDistanceService;

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
            var checkWinningBids = _repo.JobBid.FindByCondition(a => a.TruckerId == foundUser.TruckerId).ToList();
            checkWinningBids = SetKeys(checkWinningBids);
            checkWinningBids = checkWinningBids.Where(a => a.Job.Status == "Approved").ToList();
            // GET SUGGESTED BASED ON HOME ADDRESS
            if (checkWinningBids.Count < 1)
            {
                var allOpenJobs = _repo.Job.FindByCondition(a => a.Status == "Open");
                var allJobBids = _repo.JobBid.FindAll();
                var allPendingNotUser = allJobBids.Where(a => a.TruckerId != foundUser.TruckerId).Select(a => a.Job).ToList();
                var listAllOpenJobs = SetKeys(allOpenJobs.ToList());
                var listAllJobBids = SetKeys(allJobBids.ToList());
                var suggestedJobs = listAllOpenJobs.Union(allPendingNotUser).ToList();
                suggestedJobs = SetKeys(suggestedJobs);
                suggestedJobs = await SetDistance(suggestedJobs, foundUser, "HomeToSite");
                TruckerIndexViewModel model = new TruckerIndexViewModel()
                {
                    SuggestedJobs = suggestedJobs.OrderBy(a => a.Distance),
                    PendingJobs = listAllJobBids.Where(a => a.Job.Status == "Pending" && a.TruckerId == foundUser.TruckerId).Select(a => a.Job).ToList(),
                    JobsWon = listAllJobBids.Where(a => a.IsWinningBid == true && a.TruckerId == foundUser.TruckerId && a.Job.Status == "Approved").Select(a => a.Job).ToList(),
                    Trucker = foundUser
                };
                return View(model);
            }
            // GET SUGGESTED BASED ON MILL LOCATION OF WINNING BID
            else
            {
                var allOpenJobs = _repo.Job.FindByCondition(a => a.Status == "Open");
                var allJobBids = _repo.JobBid.FindAll();
                var allPendingNotUser = allJobBids.Where(a => a.TruckerId != foundUser.TruckerId).Select(a => a.Job).ToList();
                var listAllOpenJobs = SetKeys(allOpenJobs.ToList());
                var listAllJobBids = SetKeys(allJobBids.ToList());
                var suggestedJobs = listAllOpenJobs.Union(allPendingNotUser).ToList();
                suggestedJobs = SetKeys(suggestedJobs);
                suggestedJobs = await SetDistanceToNextJob(checkWinningBids.Select(a => a.Job).FirstOrDefault(), suggestedJobs);
                TruckerIndexViewModel model = new TruckerIndexViewModel()
                {
                    SuggestedJobs = suggestedJobs.OrderBy(a => a.Distance),
                    PendingJobs = listAllJobBids.Where(a => a.Job.Status == "Pending" && a.TruckerId == foundUser.TruckerId).Select(a => a.Job).ToList(),
                    JobsWon = listAllJobBids.Where(a => a.IsWinningBid == true && a.TruckerId == foundUser.TruckerId && a.Job.Status == "Approved").Select(a => a.Job).ToList(),
                    Trucker = foundUser
                };
                return View(model);
            }
        }

        // GET: Truckers/BidsWon/
        public ActionResult BidsWon()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var winningBids = _repo.JobBid.ReturnWinningBids(foundUser).ToList();
            foreach(var bid in winningBids)
            {
                bid.Job = _repo.Job.FindByCondition(a => a.JobId == bid.JobId).SingleOrDefault();
                bid.Trucker = _repo.Trucker.FindByCondition(a => a.TruckerId == bid.TruckerId).SingleOrDefault();
                bid.Job.Site = _repo.Site.FindByCondition(a => a.SiteId == bid.Job.SiteId).SingleOrDefault();
                bid.Job.Mill = _repo.Mill.FindByCondition(a => a.MillId == bid.Job.MillId).SingleOrDefault();
                bid.Job.WoodType = _repo.WoodType.FindByCondition(a => a.WoodTypeId == bid.Job.WoodTypeId).SingleOrDefault();
            }
            var result = winningBids.Where(a => a.Job.Status == "Approved");
            return View(result);
        }

        // GET: Truckers/OpenJobs
        public async Task<ActionResult> OpenJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var openJobs = _repo.Job.FindByCondition(a => a.Status == "Open").ToList();
            foreach(var job in openJobs)
            {
                job.Mill = _repo.Mill.FindByCondition(a => a.MillId == job.MillId).SingleOrDefault();
                job.Site = _repo.Site.FindByCondition(a => a.SiteId == job.SiteId).SingleOrDefault();
                job.WoodType = _repo.WoodType.FindByCondition(a => a.WoodTypeId == job.WoodTypeId).SingleOrDefault();
                var result = await _distance.GetDistance(job, foundUser, "HomeToSite");
                job.Distance = ConvertToDouble(result);
            }
            return View(openJobs);
        }

        // GET: Truckers/PendingJobs
        public ActionResult PendingJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var pendingJobs = _repo.JobBid.FindByCondition(a => a.TruckerId == foundUser.TruckerId);
            foreach (var bid in pendingJobs)
            {
                bid.Job = _repo.Job.FindByCondition(a => a.JobId == bid.JobId).SingleOrDefault();
            }
            var jobs = pendingJobs.Where(a => a.Job.Status == "Pending").Select(a => a.Job).ToList();
            foreach(var job in jobs)
            {
                job.Mill = _repo.Mill.FindByCondition(a => a.MillId == job.MillId).SingleOrDefault();
                job.Site = _repo.Site.FindByCondition(a => a.SiteId == job.SiteId).SingleOrDefault();
                job.WoodType = _repo.WoodType.FindByCondition(a => a.WoodTypeId == job.WoodTypeId).SingleOrDefault();
            }
            return View(jobs);
        }

        // GET: Truckers/Directions/5
        public ActionResult Directions(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            foundJob.Site = _repo.Site.FindByCondition(a => a.SiteId == foundJob.SiteId).SingleOrDefault();
            foundJob.Mill = _repo.Mill.FindByCondition(a => a.MillId == foundJob.MillId).SingleOrDefault();
            DirectionsViewModel model = new DirectionsViewModel()
            {
                Job = foundJob,
                Trucker = foundUser
            };
            return View(model);
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
        
        public double ConvertToDouble(DistanceJson result)
        {
            var distance = result.rows[0].elements[0].distance.text;
            distance = distance.Replace("m", "").Replace("i", "").Trim();
            var dd = double.Parse(distance);
            return dd;
        }
        public List<JobBid> SetKeys(List<JobBid> queryable)
        {
            foreach (var bid in queryable)
            {
                bid.Job = _repo.Job.FindByCondition(a => a.JobId == bid.JobId).SingleOrDefault();
                bid.Job.Mill = _repo.Mill.FindByCondition(a => a.MillId == bid.Job.MillId).SingleOrDefault();
                bid.Job.Site = _repo.Site.FindByCondition(a => a.SiteId == bid.Job.SiteId).SingleOrDefault();
                bid.Job.WoodType = _repo.WoodType.FindByCondition(a => a.WoodTypeId == bid.Job.WoodTypeId).SingleOrDefault();
            }
            return queryable;
        }
        public List<Job> SetKeys(List<Job> queryable)
        {
            foreach (var bid in queryable)
            {
                bid.Mill = _repo.Mill.FindByCondition(a => a.MillId == bid.MillId).SingleOrDefault();
                bid.Site = _repo.Site.FindByCondition(a => a.SiteId == bid.SiteId).SingleOrDefault();
                bid.WoodType = _repo.WoodType.FindByCondition(a => a.WoodTypeId == bid.WoodTypeId).SingleOrDefault();
            }
            return queryable;
        }
        public List<Job> RemoveUserBids(List<Job> jobs, List<Job> jobsToRemove)
        {
            return jobs.Union(jobsToRemove).ToList();
        }
        public async Task<List<Job>> SetDistance(List<Job> jobs, Trucker foundUser, string direction)
        {
            foreach (var job in jobs)
            {
                var result = await _distance.GetDistance(job, foundUser, direction);
                job.Distance = ConvertToDouble(result);
            }
            return jobs;
        }

        public async Task<List<Job>> SetDistanceToNextJob(Job job, List<Job> jobs)
        {
            foreach(var item in jobs)
            {
                var result = await _distance.GetDistanceToNextJob(job, item);
                item.Distance = ConvertToDouble(result);
            }
            return jobs;
        }
    }
}