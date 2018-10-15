using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcConcepts.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Tickets> Ticket { get; set; }
    }
}