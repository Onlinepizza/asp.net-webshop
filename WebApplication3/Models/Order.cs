using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    //public class Order : IEnumerable<Orderline>
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
     
    }
}