using ECommerceSite.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ECommerceSite.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ECommerceSite.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.SeedProducts(context);
        }

        private void SeedProducts(ApplicationDbContext context)
        {
            if (context.Products.Any())
            {
                return;
            }

            Product firstProduct = new Product
            {
                Name = "NIVEA® Raspberry Rosé Lip Butter",
                Description = "The moisturising formula of NIVEA Lip Butter with Hydra IQ contains Shea Butter and Almond Oil and provides intense moisture and long-lasting care.",
                Price = 1.99m
            };

            context.Products.Add(firstProduct);

            Product secondProduct = new Product
            {
                Name = "NIVEA® Sensitive Body Lotion",
                Description = "Would you like to give your sensitive skin a special treatment? NIVEA® Sensitive Body Lotion cares for your skin: Its pH neutral formula contains no colourants and is especially formulated to meet the needs on sensitive skin The formula with Chamomile Extract calms and relieves your skin, restoring its moisture balance.",
                Price = 3.59m
            };

            context.Products.Add(secondProduct);

            Product thirdProduct = new Product
            {
                Name = "NIVEA® Soft",
                Description = "This highly effective, quickly absorbed formulation delivers a fresh feeling moisture boost to skin.",
                Price = 4.29m
            };

            context.Products.Add(thirdProduct);

            Product fourthProduct = new Product
            {
                Name = "NIVEA® Pure & Natural Day Cream Dry & Sensitive",
                Description = "New NIVEA FACE CARE Pure & Natural Soothing Day Cream for beautiful looking skin What NIVEA FACE CARE",
                Price = 5.39m
            };

            context.Products.Add(fourthProduct);

            Product fiftProduct = new Product
            {
                Name = "NIVEA® Daily Essentials Caring Micellar Water Normal Skin",
                Description = "Fall in love with the magical cleansing sensation of NIVEA® Daily Essentials Sensitive Caring Micellar Water. ",
                Price = 3.89m
            };

            context.Products.Add(fiftProduct);

            context.SaveChanges();
        }
    }
}
