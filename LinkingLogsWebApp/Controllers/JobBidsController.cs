﻿using System;
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
using Stripe;

namespace LinkingLogsWebApp.Controllers
{
    public class JobBidsController : Controller
    {
        private IRepositoryWrapper _repo;
        public JobBidsController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GET: Jobs/Bid/5
        [Authorize(Roles = "Trucker")]
        public ActionResult Bid(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == id).SingleOrDefault();
            JobBidViewModel jobBid = new JobBidViewModel()
            {
                Job = foundJob,
                JobBid = new JobBid()
            };
            return View(jobBid);
        }

        // POST: Jobs/Bid
        [Authorize(Roles = "Trucker")]
        [HttpPost]
        public ActionResult Bid(JobBid jobBid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var job = _repo.Job.ReturnJob(jobBid);
            try
            {
                job.Status = "Pending";
                _repo.Job.Update(job);
                jobBid.TruckerId = foundUser.TruckerId;
                _repo.JobBid.Create(jobBid);
                _repo.Save();
                return RedirectToAction("Index", "Truckers");
            }
            catch
            {
                return View();
            }
        }

        // GET: JobBids/ApproveBid/5
        public ActionResult ApproveBid(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJobBid = _repo.JobBid.FindByCondition(a => a.JobBidId == id).SingleOrDefault();
            var foundTrucker = _repo.Trucker.FindByCondition(a => a.TruckerId == foundJobBid.TruckerId).SingleOrDefault();
            ApproveBidModel model = new ApproveBidModel()
            {
                JobBid = foundJobBid,
                Trucker = foundTrucker
            };
            return View(model);
        }

        // POST: JobBids/ApproveBid/5
        [HttpPost]
        public ActionResult ApproveBid(int id, JobBid jobBid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.SiteManager.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var foundJobBid = _repo.JobBid.FindByCondition(a => a.JobBidId == id).SingleOrDefault();
            var foundJob = _repo.Job.FindByCondition(a => a.JobId == foundJobBid.JobId).SingleOrDefault();
            foundJob.Status = "Approved";
            _repo.Job.Update(foundJob);
            foundJobBid.IsWinningBid = true;
            _repo.JobBid.Update(foundJobBid);
            _repo.Save();
            return RedirectToAction("Index", "SiteManagers");
        }

        // GET: JobBids/Pay/5
        public ActionResult Pay(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foundUser = _repo.Trucker.FindByCondition(a => a.IdentityUserId == userId).SingleOrDefault();
            var jobBid = _repo.JobBid.FindByCondition(a => a.JobId == id).SingleOrDefault();
            jobBid.Job = _repo.Job.FindByCondition(a => a.JobId == jobBid.JobId).SingleOrDefault();
            var intent = new PaymentIntent();
            return View(jobBid);
        }

        // POST: JobBids/Pay
        [HttpPost]
        public ActionResult Pay(string stripeToken)
        {
            StripeConfiguration.ApiKey = $"{ApiKeys.StripeSecretKey}";

            var options = new ChargeCreateOptions
            {
                Amount = 2000,
                Currency = "usd",
                Source = stripeToken,
                Description = "My First Test Charge (created for API docs)",
            };
            var service = new ChargeService();
            service.Create(options);
            return RedirectToAction("Index","Truckers");
        }

        // GET: JobBids
        public ActionResult Index()
        {
            return View();
        }

        // GET: JobBids/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: JobBids/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobBids/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: JobBids/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: JobBids/Edit/5
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

        // GET: JobBids/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JobBids/Delete/5
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