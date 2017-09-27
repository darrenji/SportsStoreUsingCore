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
    }
}
