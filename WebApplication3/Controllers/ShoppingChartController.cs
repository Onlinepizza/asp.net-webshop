using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

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

    }
}