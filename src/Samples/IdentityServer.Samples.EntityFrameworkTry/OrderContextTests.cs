using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IdentityServer.Samples.EntityFrameworkTry
{
    public class OrderContextTests
    {
        [Fact]
        public void Test1()
        {
            using (var context = new OrderContext())
            {
                var list = context.Orders.ToList();

                var orderDetails = context.OrderDetails.Include(o => o.Order).ToList();

                var orders = context.Orders.Include(x => x.OrderDetails);

                var order1 = new Order()
                {
                    CustomerID = 6,
                    EmployeeID = 8
                };

                var order2 = new Order()
                {
                    CustomerID = 10,
                    EmployeeID = 1,
                    OrderDate = new DateTime(2017, 12, 20)
                };

                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.SaveChanges();
            }
        }

        [Fact]
        public void AddOrderDetails()
        {
            using (var context = new OrderContext())
            {
                var order = new Order()
                {
                    CustomerID = 1,
                    EmployeeID = 2,
                    OrderDate = DateTime.Now
                };
                var orderDetail = new OrderDetail()
                {
                    Order = order,
                    Quantity = 1,
                    ProductID = 1
                };

                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
            }
        }

        [Fact]
        public void AddOrderDetail2()
        {
            using (var context = new OrderContext())
            {
                var order = context.Orders.First();

                var orderDetail = new OrderDetail()
                {
                    OrderID = order.OrderID,
                    Quantity = 3,
                    ProductID = 3
                };

                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
            }
        }

        [Fact]
        public void QueryOrderDetail()
        {
            using (var context = new OrderContext())
            {
                var order = context.Orders
                    .Include(x => x.OrderDetails)
                    .First(x => x.CustomerID == 6);
                var id = order.OrderID;
            }
        }
    }
}