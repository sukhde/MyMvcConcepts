namespace MvcConcepts.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MvcConcepts.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcConcepts.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "MvcConcepts.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            context.TicketTypes.AddOrUpdate(p => p.Id,
           new TicketType() { Id = 1, Title = "Software Update" },
           new TicketType() { Id = 2, Title = "Database Eror" },
           new TicketType() { Id = 3, Title = "Bug fixes" },
           new TicketType() { Id = 4, Title = "Adding Helpers" });
            context.TicketPriorities.AddOrUpdate(p => p.Id,
            new TicketPriority() { Id = 1, Title = "Low" },
            new TicketPriority() { Id = 2, Title = "Medium" },
            new TicketPriority() { Id = 3, Title = "High" },
            new TicketPriority() { Id = 4, Title = "Urgent" });

            context.TicketStatus.AddOrUpdate(p => p.Id,
            new TicketStatus() { Id = 1, Title = "started" },
            new TicketStatus() { Id = 2, Title = "finished" },
            new TicketStatus() { Id = 3, Title = "completed" },
            new TicketStatus() { Id = 4, Title = "incomplete" });



            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Check if Roles already exist on the database/
            //If not, create them.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole("Admin");

                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            ApplicationUser adminUser = null;
            ApplicationUser SubmitterUser = null;
            ApplicationUser DeveloperUser = null;
            ApplicationUser ProjectManager = null;
            //Check if user exists on the database. 
            //If not, create it. 
            if (!context.Users.Any(p => p.UserName == "admin@myblogapp.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "admin@myblogapp.com";
                adminUser.Email = "admin@myblogapp.com";
                adminUser.FirstName = "Admin";
                adminUser.LastName = "User";
                adminUser.DisplayName = "Admin";

                userManager.Create(adminUser, "Deep211@");
            }
            else
            {
                //Get user from the database
                adminUser = context.Users.Where(p => p.UserName == "admin@myblogapp.com")
                    .FirstOrDefault();
            }

            if (!context.Users.Any(p => p.UserName == "Sub@myblogapp.com"))
            {
                SubmitterUser = new ApplicationUser();
                SubmitterUser.UserName = "Sub@myblogapp.com";
                SubmitterUser.Email = "Sub@myblogapp.com";
                SubmitterUser.FirstName = "Submitter";
                SubmitterUser.LastName = "User";
                SubmitterUser.DisplayName = "Submitter";

                userManager.Create(SubmitterUser, "Deep23@");
            }
            else
            {
                //Get user from the database
                SubmitterUser = context.Users.Where(p => p.UserName == "Sub@myblogapp.com")
                    .FirstOrDefault();
            }

            if (!context.Users.Any(p => p.UserName == "DeveloperUser@myblogapp.com"))
            {
                DeveloperUser = new ApplicationUser();
                DeveloperUser.UserName = "DeveloperUser@myblogapp.com";
                DeveloperUser.Email = "DeveloperUser@myblogapp.com";
                DeveloperUser.FirstName = "Developer";
                DeveloperUser.LastName = "User";
                DeveloperUser.DisplayName = "Developer";

                userManager.Create(DeveloperUser, "Deep1@");
            }
            else
            {
                //Get user from the database
                DeveloperUser = context.Users.Where(p => p.UserName == "DeveloperUser@myblogapp.com")
                    .FirstOrDefault();
            }

            if (!context.Users.Any(p => p.UserName == "ProjectManager@myblogapp.com"))
            {
                ProjectManager = new ApplicationUser();
                ProjectManager.UserName = "ProjectManager@myblogapp.com";
                ProjectManager.Email = "ProjectManager@myblogapp.com";
                ProjectManager.FirstName = "ProjectManager";
                ProjectManager.LastName = "User";
                ProjectManager.DisplayName = "ProjectManager";

                userManager.Create(ProjectManager, "Deep2@");
            }
            else
            {
                //Get user from the database
                ProjectManager = context.Users.Where(p => p.UserName == "ProjectManager@myblogapp.com")
                    .FirstOrDefault();
            }
            //Check if the adminUser is already on the Admin role
            //If not, add it.
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
            if (!userManager.IsInRole(DeveloperUser.Id, "Developer"))
            {
                userManager.AddToRole(DeveloperUser.Id, "Developer");
            }
            if (!userManager.IsInRole(ProjectManager.Id, "Project Manager"))
            {
                userManager.AddToRole(ProjectManager.Id, "Project Manager");
            }
            if (!userManager.IsInRole(SubmitterUser.Id, "Submitter"))
            {
                userManager.AddToRole(SubmitterUser.Id, "Submitter");
            }

        }
    }
}
