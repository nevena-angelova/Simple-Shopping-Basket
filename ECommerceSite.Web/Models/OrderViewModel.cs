using System.Collections.Generic;

namespace ECommerceSite.Web.Models
{
    public class OrderViewModel
    {
        public decimal TotalPrice { get; set; }

        public ICollection<OrderProductViewModel> Products { get; set; }
    }
}