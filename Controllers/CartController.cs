using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStoreUsingCore.Models;
using Microsoft.AspNetCore.Http;
using SportsStoreUsingCore.Infrastructure;
using Newtonsoft.Json;
using SportsStoreUsingCore.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsStoreUsingCore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;

        public CartController(IProductRepository repo)
        {
            repository = repo;
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if(product!=null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }


        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if(product!=null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }

            //转到购物车页的时候都带着这个url
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
  

        private Cart GetCart()
        {
            //没有扩展方法之前的写法
            //Cart cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("Cart")) ?? new Cart();
            //return cart;
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart)
        {
            //没有扩展方法的写法
            //HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}
