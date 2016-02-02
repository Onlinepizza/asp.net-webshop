using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace WebApplication3.Models
{
    public class Product
    {
        
        public int ProductID { get; set; }
        
        public string ArtName { get; set; }
        public string Descr { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }

    }

   
}