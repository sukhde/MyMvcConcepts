using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcConcepts.Models
{
    public class TicketAssignViewModels
    {
        public int Id { get; set; }
        public SelectList UserList { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}