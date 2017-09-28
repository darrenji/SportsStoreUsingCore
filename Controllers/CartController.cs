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
        private Cart cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        //这个returnUrl在点击产品加入购物车按钮的时候带来，再寸放到视图页面
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            #region 没有把购物车放到依赖倒置容器的写法
            //if (product != null)
            //{
            //    Cart cart = GetCart();
            //    cart.AddItem(product, 1);
            //    SaveCart(cart);
            //} 
            #endregion

            if (product != null)
            {
                cart.AddItem(product, 1);
            }


            return RedirectToAction("Index", new { returnUrl });
        }


        //购物车页面的每一次操作都要把视图页的returnUrl带过来，再回到视图页存起来
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            #region 没有把购物车放到依赖倒置容器的写法
            //if(product!=null)
            //{
            //    Cart cart = GetCart();
            //    cart.RemoveLine(product);
            //    SaveCart(cart);
            //} 
            #endregion

            if(product!=null)
            {
                cart.RemoveLine(product);
            }

            //转到购物车页的时候都带着这个url
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(string returnUrl)
        {
            #region 没有把购物车放到依赖倒置容器的写法
            //return View(new CartIndexViewModel
            //{
            //    Cart = GetCart(),
            //    ReturnUrl = returnUrl
            //}); 
            #endregion

            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }


        #region 没有把购物车放到依赖倒置容器的写法
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
        #endregion
    }
}
