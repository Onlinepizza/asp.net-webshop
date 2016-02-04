using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ShoppingChartController : Controller
    {
        ShoppingChart chart;

        // GET: ShoppingChart
        public ActionResult Index()
        {
            chart = ShoppingChart.getInstance();
            chart.AddProductToChart(1, 5);

            return View(chart.getChartObjectProdName());
        }
    }
}