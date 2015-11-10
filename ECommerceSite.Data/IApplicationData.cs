using ECommerceSite.Data.Repositories;
using ECommerceSite.Models;
using System.Data.Entity;

namespace ECommerceSite.Data
{
    public interface IApplicationData
    {
        DbContext Context { get; }

        IRepository<ApplicationUser> Users { get; }

        IRepository<Product> Products { get; }

        IRepository<BasketItem> BasketItems { get; }

        void Dispose();

        int SaveChanges();
    }
}
