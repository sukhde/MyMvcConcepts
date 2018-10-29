using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcConcepts.Helper;
using MvcConcepts.Models;

namespace MvcConcepts.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Submitter")]
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
        [Authorize(Roles = "Submitter")]
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
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title", tickets.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title", tickets.TicketTypeId);
            return View(tickets);
        }
        //Create/comment
        [HttpPost]
        [Authorize]
        public ActionResult CreateComment(int id, string body)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Tickets = db.Tickets
               .Where(p => p.Id == id)
               .FirstOrDefault();
            if (Tickets == null)
            {
                return HttpNotFound();
            }
            var comment = new TicketComments();
            comment.UserId = User.Identity.GetUserId();
            comment.TicketId = Tickets.Id;
            comment.Created = DateTime.Now;
            comment.Comment = body;
            db.TicketComments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id });
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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,CreatorId,AssigneeId")] Tickets ticket)
        {
            if (ModelState.IsValid)
            {
                var dateChanged = DateTimeOffset.Now;
                var changes = new List<TicketHistory>();
                var dbTicket = db.Tickets.First(p => p.Id == ticket.Id);
                dbTicket.Title = ticket.Title;
                dbTicket.Description = ticket.Description;
                dbTicket.TicketTypeId = ticket.TicketTypeId;
                dbTicket.Updated = dateChanged;
                var originalValues = db.Entry(dbTicket).OriginalValues;
                var currentValues = db.Entry(dbTicket).CurrentValues;

                foreach (var property in originalValues.PropertyNames)
                {
                    if (property != "Updated")
                    {
                        var originalValue = originalValues[property]?.ToString();
                        var currentValue = currentValues[property]?.ToString();
                        if (originalValue != currentValue)
                        {
                            var history = new TicketHistory();
                            history.Changed = dateChanged;
                            history.NewValue = GetValueFromKey(property, currentValue);
                            history.OldValue = GetValueFromKey(property, originalValue);
                            history.Property = property;
                            history.TicketId = dbTicket.Id;
                            history.UserId = User.Identity.GetUserId();
                            changes.Add(history);
                        }
                    }
                }
                db.TicketHistories.AddRange(changes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", ticket.AssigneeId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticket.CreatorId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }
        private string GetValueFromKey(string propertyName, string key)
        {
            if (propertyName == "TicketTypeId")
            {
                return db.TicketTypes.Find(Convert.ToInt32(key)).Title;
            }

            if (propertyName == "TicketPriorityId")
            {
                return db.TicketPriorities.Find(Convert.ToInt32(key)).Title;
            }

            if (propertyName == "TicketStatusId")
            {
                return db.TicketStatus.Find(Convert.ToInt32(key)).Title;
            }

            if (propertyName == "ProjectId")
            {
                return db.Projects.Find(Convert.ToInt32(key)).Name;
            }
            return key;
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

        // GET: TicketAttachments/Create
        public ActionResult Attachment()
        {

            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Attachment([Bind(Include = "TicketId,Description,Created,UserId,FileUrl")]int id, TicketAttachments ticketAttachments, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    ticketAttachments.FileUrl = "/Uploads/" + fileName;
                }
                ticketAttachments.Created = DateTime.Now;
                ticketAttachments.UserId = User.Identity.GetUserId();
                ticketAttachments.TicketId = id;
                db.TicketAttachments.Add(ticketAttachments);
                db.SaveChanges();

                var Tickets = db.Tickets.Where(x => x.Id == ticketAttachments.TicketId).FirstOrDefault();
                if (Tickets.AssigneeId != null)
                {
                    var personalEmailService = new PersonalEmailService();
                    var mailMessage = new MailMessage(WebConfigurationManager.AppSettings["emailto"], Tickets.Assignee.Email);
                    mailMessage.Body = "added attachment";
                    mailMessage.IsBodyHtml = true;
                    personalEmailService.Send(mailMessage);
                }


                return RedirectToAction("Index", "Tickets");
            }


            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", ticketAttachments.UserId);
            return View(ticketAttachments);

        }
        //Get //submitter tickets
        [Authorize(Roles = "Submitter")]
        public ActionResult SubmitterTickets()
        {
            var UserId = User.Identity.GetUserId();
            var tickets = db.Tickets
                .Where(t => t.CreatorId == UserId).Include(t => t.Assignee).Include(t => t.Creator).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View("Index", tickets.ToList());
        }


        //Get //Developer tickets
        [Authorize(Roles = "Developer")]
        public ActionResult DeveloperTickets()
        {
            var UserId = User.Identity.GetUserId();
            var tickets = db.Tickets
                .Where(t => t.AssigneeId == UserId).Include(t => t.Assignee).Include(t => t.Creator).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View("Index", tickets.ToList());
        }


        //Assignticket 
        public ActionResult AssignTickets(int id)
        {
            var model = new TicketAssignViewModels();

            var ticket = db.Tickets.FirstOrDefault(p => p.Id == id);
            var users = db.Users.ToList();
            var RoleAssigned = ticket.AssigneeId;
            model.Id = id;
            model.UserList = new SelectList(users, "Id", "Name", RoleAssigned);
            return View(model);
        }
        [HttpPost]
        public ActionResult AssignTickets(TicketAssignViewModels model)
        {
            var ticket = db.Tickets.FirstOrDefault(p => p.Id == model.Id);
            if (model.SelectedUsers != null)
            {
                foreach (var userId in model.SelectedUsers)
                {
                    var user = db.Users.FirstOrDefault(p => p.Id == userId);
                    ticket.AssigneeId = userId;
                }
            }
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
