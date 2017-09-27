using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStoreUsingCore.Models
{
    public class Cart
    {
        private List<Cartline> lineCollection = new List<Cartline>();

        public virtual void AddItem(Product product, int quantity)
        {
            Cartline line = lineCollection.Where(p => p.Product.ProductID == product.ProductID).FirstOrDefault();
            if(line==null)
            {
                lineCollection.Add(new Cartline { Product=product, Quantity=quantity});
            } else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public virtual decimal ComputeTotalValue() =>
            lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() =>
            lineCollection.Clear();

        public virtual IEnumerable<Cartline> Lines => lineCollection;
    }

    public class Cartline
    {
        public int CarLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
