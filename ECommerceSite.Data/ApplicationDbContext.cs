using ECommerceSite.Data.Migrations;
using ECommerceSite.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ECommerceSite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ECommerceSite", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<BasketItem> BasketItems { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
