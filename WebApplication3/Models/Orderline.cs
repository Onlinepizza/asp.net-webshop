using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Collections;

namespace WebApplication3.Models
{

    public class Orderline
    {   
        public int OrderlineID { get; set; }
        public int ArtID { get; set; }
        public int OrderID { get; set; }
        public int Antal { get; set; }
    }
}