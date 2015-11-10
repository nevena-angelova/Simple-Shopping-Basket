using AutoMapper.QueryableExtensions;
using ECommerceSite.Data;
using ECommerceSite.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ECommerceSite.Models;
using System.Net;
using System.Web.Script.Serialization;

namespace ECommerceSite.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IApplicationData data) : base(data)
        { }

        /// <summary>
        /// Lists all the products
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<ProductViewModel> products = this.Data.Products.All()
                .ProjectTo<ProductViewModel>()
                .ToList();

            return View(products);
        }

        /// <summary>
        /// Adds a product to the current user's basket if it does not
        /// exist in the basket, othewise its amount is raised with one.
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <returns>The basket items as json</returns>
        public JsonResult AddToBasket(int productId)
        {
            var currentUser = this.Data.Users.All()
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            var item = currentUser.BasketItems.Where(bi => bi.ProductId == productId).FirstOrDefault();
            if (item == null)
            {
                var newProduct = this.Data.Products.All()
                    .Where(p => p.Id == productId).FirstOrDefault();

                if (newProduct == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.Json("Product not found!");
                }

                currentUser.BasketItems.Add(new BasketItem { Product = newProduct });
            }
            else
            {
                item.Amount++;
            }

            var outputProducts = currentUser.BasketItems
                .AsQueryable()
                .ProjectTo<BasketProductViewModel>();

            this.Data.SaveChanges();

            return this.Json(outputProducts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action is called from the Register/Login actions.
        /// Products as string, generated from the local storage are converted and added to the database.
        /// If there are duplicates their amounts are summed.
        /// Finally a delete local storage option is passed by 'TempData'.
        /// </summary>
        /// <param name="products">Products as string, generated from the local storage</param>
        /// <returns>Redirects to the Index</returns>
        public ActionResult AddToBasketLS(string products)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var basketProducts = serializer.Deserialize<Dictionary<string, int>>(products);

            var curentUser = this.Data.Users.All()
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (basketProducts != null)
            {
                foreach (var product in basketProducts)
                {
                    var item = curentUser.BasketItems
                        .Where(bi => bi.ProductId.ToString() == product.Key)
                        .FirstOrDefault();

                    if (item != null)
                    {
                        item.Amount += product.Value;
                    }
                    else
                    {
                        curentUser.BasketItems.Add(
                            new BasketItem
                            {
                                ProductId = int.Parse(product.Key),
                                Amount = product.Value
                            });
                    }
                }
            }

            this.Data.SaveChanges();

            //When a user is registered or logged in the local storage is cleared.
            TempData["Clear"] = "Yes";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns the products from the current user's basket 
        /// </summary>
        /// <returns>The basket items as json</returns>
        public JsonResult GetBasketItems()
        {
            var userBasketItems = this.Data.BasketItems.All()
                .Where(bi => bi.User.UserName == User.Identity.Name)
                .ProjectTo<BasketProductViewModel>();

            return this.Json(userBasketItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes a product from the current user's basket if
        /// its amount is smaller or equal to 1 otherwise reduces it with one.
        /// </summary>
        /// <param name="productId">the product's id</param>
        /// <returns>The basket items as json</returns>
        public JsonResult RemoveFromBasket(int productId)
        {
            var itemToDelete = this.Data.BasketItems.All()
                .Where(bi => bi.ProductId == productId && bi.User.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (itemToDelete.Amount <= 1)
            {
                this.Data.BasketItems.Delete(itemToDelete);
            }
            else
            {
                itemToDelete.Amount--;
            }

            this.Data.SaveChanges();

            var outputBasketItems = this.Data.BasketItems.All()
               .Where(bi => bi.User.UserName == User.Identity.Name)
               .ProjectTo<BasketProductViewModel>();

            return this.Json(outputBasketItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// If the user is authenticated removes all items from the database basket.
        /// </summary>
        /// <param name="order">order view model</param>
        /// <returns>the order as json</returns>
        public JsonResult ConfirmOrder(OrderViewModel order)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = this.Data.Users.All()
                    .Where(u => u.UserName == User.Identity.Name)
                    .FirstOrDefault();

                currentUser.BasketItems.Clear();
                this.Data.SaveChanges();
            }

            return this.Json(order, JsonRequestBehavior.AllowGet);
        }
    }
}