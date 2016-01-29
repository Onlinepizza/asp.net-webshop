using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public int CustomerID { get; set; }
        
    }


    public class OrderDBContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
    }
}