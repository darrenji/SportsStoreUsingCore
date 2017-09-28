using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStoreUsingCore.Models
{
    public class FakeRepository : IProductRepository
    {
        private ApplicationDbContext _context;

        public FakeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> Products => _context.Products.ToList();

        public Product DeleteProduct(int productId)
        {
            Product dbEntry = _context.Products.FirstOrDefault(t => t.ProductID == productId);
            if(dbEntry!=null)
            {
                _context.Products.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveProduct(Product product)
        {
            if(product.ProductID==0)
            {
                _context.Products.Add(product);
            }
            else
            {
                Product dbEntry = _context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if(dbEntry!=null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
                
            }
            _context.SaveChanges();
        }
    }
}
