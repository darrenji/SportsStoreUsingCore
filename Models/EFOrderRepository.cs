using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStoreUsingCore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public IEnumerable<Order> GetOrdersBySql(string sqlStr)
        {
            return context.Orders.FromSql(sqlStr).ToList();
        }

        public void SaveOrder(Order order)
        {
            //在购物车页面，是从Session中获取购物车的，而从Session中获取购物车实例的过程，本质上是NewtonSoft.Json通过反序列化一个Cart实例出来的，但，这个反序列化出来的Cart实例是拷贝出来的一个新实例， Entity Framework Core根本不知道，也就是，此时，当EF Core和数据库交互的时候还不知道一些发生的变化。对Product来说，数据库层面没有变，但反序列出来的时候已经变了，这是后保存变化就会报错。如何避免呢？AttacheRange就是用来解决这个问题的，告诉ef core不要做什么，除非Product有变化。
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
