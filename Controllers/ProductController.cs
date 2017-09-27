using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStoreUsingCore.Models;
using SportsStoreUsingCore.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsStoreUsingCore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult List(int page=1) => View(new ProductListViewModel {
            Products = repository.Products
                .OrderBy(p => p.ProductID)
                .Skip(PageSize * (page-1))
                .Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = repository.Products.Count()
            }
        });
    }
}
