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

        protected override void Seed(MvcConcepts.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
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
           
            //Check if user exists on the database.
            //If not, create it. 
            if (!context.Users.Any(p => p.UserName == "admin@myblogapp.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "admin@myblogapp.com";
                adminUser.Email = "admin@myblogapp.com";
                adminUser.FirstName = "Admin";
                adminUser.LastName = "User";
                adminUser.DisplayName = "Admin User";

                userManager.Create(adminUser, "Deep211@");
            }
            else
            {
                //Get user from the database
                adminUser = context.Users.Where(p => p.UserName == "admin@myblogapp.com")
                    .FirstOrDefault();
            }


            //Check if the adminUser is already on the Admin role
            //If not, add it.
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
           
        }
    }
}
