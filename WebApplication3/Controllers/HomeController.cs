﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using WebApplication3.Models;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private TheDatabase db = new TheDatabase();

        // GET: Home
        public ActionResult Index()
        {

            InitializeShoppingCartCookie();

                return View();    
                 
        }

        private void InitializeShoppingCartCookie()
        {
            string cookieValue = null;

            if (Request.Cookies[CookieModel.CookieName] == null)
                Response.Cookies[CookieModel.CookieName].Value = CookieModel.GetNextCartNameEncoded();

            cookieValue = Request.Cookies[CookieModel.CookieName].Value;

            ShoppingChart.InitializeShoppingChart(cookieValue);
        }

        //[ScriptMethod, WebMethod]
        //public static void HandleClose()
        //{
        //    System.Diagnostics.Debug.WriteLine("success");
        //    //lägg tillbaks produkter i stock och avsluta applikationen
        //}

        //GET: Order
       
    }
}