﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcConcepts.Models
{
    public class TicketNotifications
    {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public virtual Tickets Ticket { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}