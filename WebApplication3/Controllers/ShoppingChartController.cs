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
            return View(product);
        }

        public ActionResult LastAddedProduct(int? id)
        {
            return View(ShoppingChart.getInstance().LastAddedProduct(id));
        }

    }
}