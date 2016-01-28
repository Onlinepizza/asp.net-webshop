using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace WebApplication3.Models
{
    public class Customer
    {

        public int CustomerID { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
        public string Adress { get; set; }
        public int PostNr { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }


    }

    public class CostumerDBContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}