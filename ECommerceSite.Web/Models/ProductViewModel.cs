using ECommerceSite.Models;
using ECommerceSite.Web.Infrastructure.Mappings;

namespace ECommerceSite.Web.Models
{
    public class ProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}