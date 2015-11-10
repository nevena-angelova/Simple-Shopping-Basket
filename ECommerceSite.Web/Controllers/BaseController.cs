using ECommerceSite.Data;
using System.Web.Mvc;

namespace ECommerceSite.Web.Controllers
{
    [HandleError]
    public class BaseController : Controller
    {
        protected IApplicationData Data { get; private set; }

        public BaseController(IApplicationData data)
        {
            this.Data = data;
        }
    }
}