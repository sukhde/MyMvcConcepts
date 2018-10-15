﻿using MvcConcepts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcConcepts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = new ApplicationUser();
            var db = new ApplicationDbContext();
            user.CreatedTickets.ToList();
            user.AssignedTickets.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}