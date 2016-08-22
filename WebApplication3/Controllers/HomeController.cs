using System;
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

        //GET: Order
       
    }
}