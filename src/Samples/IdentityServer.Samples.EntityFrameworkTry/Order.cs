using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Samples.EntityFrameworkTry
{
    public class OrderDetail
    {
        [Key] public int OrderDetailID { get; set; }
        [Required] public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        
        [Timestamp]
        public byte[] TStamp { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime OrderDate { get; set; }
        
        public string Version { get; set; }
        
        public List<OrderDetail> OrderDetails { get; set; }
    }
}