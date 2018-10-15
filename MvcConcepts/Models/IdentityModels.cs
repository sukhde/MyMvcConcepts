using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MvcConcepts.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
            public string Name { get; set; }

            public ApplicationUser()
            {
            Projects = new HashSet<Project>();
            CreatedTickets = new HashSet<Tickets>();
            AssignedTickets = new HashSet<Tickets>();
        }

            public virtual ICollection<Project> Projects { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<Tickets> CreatedTickets { get; set; }
        [InverseProperty("Assignee")]
        public virtual ICollection<Tickets> AssignedTickets { get; set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string DisplayName { get; internal set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
            {
                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here
                return userIdentity;
            }
        }

        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext()
                : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }

            public System.Data.Entity.DbSet<MvcConcepts.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<MvcConcepts.Models.Tickets> Tickets { get; set; }

        public System.Data.Entity.DbSet<MvcConcepts.Models.TicketPriority> TicketPriorities { get; set; }

        public System.Data.Entity.DbSet<MvcConcepts.Models.TicketStatus> TicketStatus { get; set; }

        public System.Data.Entity.DbSet<MvcConcepts.Models.TicketType> TicketTypes { get; set; }
    }
    }