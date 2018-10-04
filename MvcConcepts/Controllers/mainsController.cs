using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcConcepts.Models;

namespace MvcConcepts.Controllers
{
    public class mainsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: mains
        public ActionResult Index()
        {
            return View(db.mains.ToList());
        }

        // GET: mains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            main main = db.mains.Find(id);
            if (main == null)
            {
                return HttpNotFound();
            }
            return View(main);
        }

        // GET: mains/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] main main)
        {
            if (ModelState.IsValid)
            {
                db.mains.Add(main);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(main);
        }

        // GET: mains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            main main = db.mains.Find(id);
            if (main == null)
            {
                return HttpNotFound();
            }
            return View(main);
        }

        // POST: mains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] main main)
        {
            if (ModelState.IsValid)
            {
                db.Entry(main).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(main);
        }

        // GET: mains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            main main = db.mains.Find(id);
            if (main == null)
            {
                return HttpNotFound();
            }
            return View(main);
        }

        // POST: mains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            main main = db.mains.Find(id);
            db.mains.Remove(main);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
