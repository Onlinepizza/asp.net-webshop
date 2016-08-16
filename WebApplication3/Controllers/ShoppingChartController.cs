using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;


/*
 * 
 *
 * Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes("your cookie value")))

Encoding.UTF8.GetString(MachineKey.Unprotect(Convert.FromBase64String("your cookie value")))



    Request.Cookies["someCookie"]



    HttpCookie cookie = new HttpCookie("search");

will reset the search cookie

To get a cookie:

HttpCookie cookie = HttpContext.Request.Cookies.Get("some_cookie_name");

To check for a cookie's existence:

HttpContext.Request.Cookies["some_cookie_name"] != null

To save a cookie:

HttpCookie cookie = new HttpCookie("some_cookie_name");
HttpContext.Response.Cookies.Remove("some_cookie_name");
HttpContext.Response.SetCookie(cookie );


 * */

namespace WebApplication3.Controllers
{
    public class ShoppingChartController : Controller
    {
        private TheDatabase db = new TheDatabase();
        // GET: ShoppingChart
        public ActionResult Index()
        {

            ViewData["TotalExclTax"] = ShoppingChart.getInstance().TotalSumExclTax();
            ViewData["TotalInclTax"] = ShoppingChart.getInstance().TotalSumInclTax();

            return View(ShoppingChart.getInstance());
        }


        // GET: Products/Details/5
        public ActionResult CustomerDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewData["action"] = "Index";
            ViewData["controller"] = "ShoppingChart";

            return View(product);
        }

        public ActionResult LastAddedProduct()
        {
            return View(ShoppingChart.getInstance().LastAddedProduct());
        }

        public ActionResult CheckoutProducts()
        {
            string answer = ShoppingChart.getInstance().CheckoutProducts();

            ViewData["answer"] = answer;
            return View();
        }
    }
}