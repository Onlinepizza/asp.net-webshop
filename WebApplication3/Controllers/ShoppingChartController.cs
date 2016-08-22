using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
            ViewData["TotalExclTax"] = 0;
            ViewData["TotalInclTax"] = 0;

            string cookieValue;

            if (CookieModel.IsCookieValid(Request, out cookieValue))
            {
                ViewData["TotalExclTax"] = ShoppingChart.getInstance().TotalSumExclTax(cookieValue);
                ViewData["TotalInclTax"] = ShoppingChart.getInstance().TotalSumInclTax(cookieValue);
            }

            return View(ShoppingChart.getInstance().GetEnumerator(cookieValue));
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
            ChartObject chartObject = new ChartObject();
            string cookieValue;

            if (CookieModel.IsCookieValid(Request, out cookieValue))
                chartObject = ShoppingChart.getInstance().LastAddedProduct(cookieValue);

            return View(chartObject);
        }

        public ActionResult CheckoutProducts()
        {
            string answer = "";

            string cookieValue;

            if (CookieModel.IsCookieValid(Request, out cookieValue))
                if (ShoppingChart.getInstance().CheckoutProducts(cookieValue))
                        answer = "All went well: " + ShoppingChart.getInstance().GetOrderMessage(cookieValue);
                    else
                        answer = "Something went wrong: " + ShoppingChart.getInstance().GetOrderMessage(cookieValue);

            ViewData["answer"] = answer;

            return View();
        }
    }
}