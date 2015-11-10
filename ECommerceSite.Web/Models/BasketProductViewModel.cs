using AutoMapper;
using ECommerceSite.Models;
using ECommerceSite.Web.Infrastructure.Mappings;

namespace ECommerceSite.Web.Models
{
    public class BasketProductViewModel : IMapFrom<BasketItem>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<BasketItem, BasketProductViewModel>()
            .ForMember(m => m.Id, opt => opt.MapFrom(bi => bi.ProductId))
            .ForMember(m => m.Name, opt => opt.MapFrom(bi => bi.Product.Name))
            .ForMember(m => m.Price, opt => opt.MapFrom(bi => bi.Product.Price));
        }
    }
}