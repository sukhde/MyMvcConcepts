using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcConcepts.Models
{

    public class Tickets

    {
        public Tickets()
        {
            Attachments = new HashSet<TicketAttachments>();
            Comments = new HashSet<TicketComments>();
            Histories = new HashSet<TicketHistory>();
            Notifications = new HashSet<TicketNotifications>();
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }

        public int TicketPriorityId { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }

        public int TicketStatusId { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }

        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }

        public string AssigneeId { get; set; }
        public virtual ApplicationUser Assignee { get; set; }
       
        public virtual ICollection<TicketAttachments> Attachments { get; set; }
        public virtual ICollection<TicketComments> Comments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
        public virtual ICollection<TicketNotifications> Notifications { get; set; }

    }
}