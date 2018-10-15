using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcConcepts.Models;

namespace MvcConcepts.Controllers
{[Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.Assignee).Include(t => t.Creator).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name");
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Title");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,AssignId,TicketTypeId,ProjectId,TicketPriorityId,TicketStatusId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Created = DateTime.Now;
                tickets.TicketStatusId = 1;
                tickets.CreatorId = User.Identity.GetUserId();
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title", tickets.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", tickets.AssigneeId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", tickets.CreatorId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Title", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title", tickets.TicketTypeId);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,CreatorId,AssigneeId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(tickets).State = EntityState.Modified;
               
           
                var ticketDb = db.Tickets.Where(p => p.Id == tickets.Id).FirstOrDefault();

                ticketDb.Updated = DateTime.Now;
                ticketDb.Title = tickets.Title;
                ticketDb.Description = tickets.Description;
                db.SaveChanges();
                return RedirectToAction("Index", "Tickets");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", tickets.AssigneeId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", tickets.CreatorId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Title", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
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
