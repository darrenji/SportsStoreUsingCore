using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStoreUsingCore.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsStoreUsingCore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }
        // GET: /<controller>/
        public IActionResult Checkout() => View(new Order());  
        
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if(ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }

        public ViewResult List()
        {
            try
            {
                var orders = repository.GetOrdersBySql("SELECT * FROM Orders");
                return View(orders.Where(o => !o.Shipped));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {
            try
            {
                IEnumerable<Order> orders = repository.GetOrdersBySql("SELECT * FROM Orders");
                Order order = orders.FirstOrDefault(o => o.OrderID == orderID);

                if (order != null)
                {
                    order.Shipped = true;
                    repository.SaveOrder(order);
                }
                return RedirectToAction(nameof(List));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
