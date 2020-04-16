using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkingLogsWebApp.Contracts;
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
            return View();
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