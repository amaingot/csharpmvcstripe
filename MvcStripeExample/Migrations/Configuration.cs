using MvcStripeExample.Misc;

namespace MvcStripeExample.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MvcStripeExample.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Models.ApplicationDbContext context)
        {
            CreateDefaultAdmin(context);
        }

        private bool CreateDefaultAdmin(ApplicationDbContext context)
        {
            IdentityResult ir;

            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            rm.Create(new IdentityRole("admin"));

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "admin@esyncsolutions.net",
                Email = "admin@esyncsolutions.net",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "SuperUser",
                Address = "One Boss Rd",
                Address2 = "Suite One",
                City = "Bossville",
                Zip = 10001,
                State = State.DC,
                PhoneNumber = "800-100-9999",
                PhoneNumberConfirmed = true
            };

            ir = um.Create(user, "%&T6D7nT23Ph");

            if (ir.Succeeded == false)
                return ir.Succeeded;

            ir = um.AddToRole(user.Id, "admin");

            return ir.Succeeded;
        }
    }
}
