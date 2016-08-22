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
                ViewData["TotalExclTax"] = ShoppingChart.TotalSumExclTax(cookieValue);
                ViewData["TotalInclTax"] = ShoppingChart.TotalSumInclTax(cookieValue);
            }

            return View(ShoppingChart.GetEnumerator(cookieValue));
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
                chartObject = ShoppingChart.LastAddedProduct(cookieValue);

            return View(chartObject);
        }

        public ActionResult CheckoutProducts()
        {
            string answer = "";

            string cookieValue;

            if (CookieModel.IsCookieValid(Request, out cookieValue))
                if (ShoppingChart.CheckoutProducts(cookieValue))
                        answer = "All went well: " + ShoppingChart.GetOrderMessage(cookieValue);
                    else
                        answer = "Something went wrong: " + ShoppingChart.GetOrderMessage(cookieValue);

            ViewData["answer"] = answer;

            return View();
        }
    }
}