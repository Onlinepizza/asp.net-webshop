﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private TheDatabase db = new TheDatabase();

        // GET: Home
        public ActionResult Index()
        {

                return View();    
                 
        }

        //GET: Order
       
    }
}