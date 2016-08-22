using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ProductsController : Controller
    {
        private TheDatabase db = new TheDatabase();

        // GET: Products
        public ActionResult Index(string description, string searchString)
        {
            var descList = new List<string>();
            var descQry = from p in db.Products
                          orderby p.Descr
                          select p.Descr;

            descList.AddRange(descQry.Distinct());
            ViewBag.description = new SelectList(descList);

            var products = from l in db.Products
                           select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ArtName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(description))
            {
                products = products.Where(x => x.Descr.Contains(description));
            }
            return View(products);
            //return View(db.Products.ToList());
        }

        // GET: Shopping
        public ActionResult Shopping(string description, string searchString)
        {
            var descList = new List<string>();
            var descQry = from p in db.Products
                          orderby p.Descr
                          select p.Descr;

            descList.AddRange(descQry.Distinct());
            ViewBag.description = new SelectList(descList);

            var products = from l in db.Products
                           select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ArtName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(description))
            {
                products = products.Where(x => x.Descr.Contains(description));
            }
            return View(products);
            //return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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

        // GET: Products/Details/5
        public ActionResult CustomerDetails(int? id)
        {

            int i;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var lines = from l in db.Orderlines select l;
            lines = lines.Where(s => s.ArtID == id);
            if (lines.Any()) {
                var relatedLines = from l in db.Orderlines select l;
                relatedLines = relatedLines.Where(s => s.OrderID == lines.FirstOrDefault().OrderID);
                if (relatedLines.Any()) { 
                    relatedLines = relatedLines.Where(s => s.ArtID != id);                
                    Product related = db.Products.Find(relatedLines.FirstOrDefault().ArtID);
                    ViewBag.Name = related.ArtName;
                    ViewBag.Id = related.ProductID;
                }
            }
            ViewData["action"] = "Shopping";
            ViewData["controller"] = "Products";
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ArtName,Descr,Price,InStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ArtName,Descr,Price,InStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Buy(int id, string Quantity)
        {
            int parsedQuantity = 0;
            string cookieValue;

            if (CookieModel.IsCookieValid(Request, out cookieValue) && int.TryParse(Quantity, out parsedQuantity))
                ShoppingChart.getInstance().AddProductToChart(id, parsedQuantity, cookieValue);

            return RedirectToAction("Shopping", "Products");
        }

        public ActionResult RemoveFromChart(int id)
        {
            string cookieValue;

            if (CookieModel.IsCookieValid(Request, out cookieValue))
                ShoppingChart.getInstance().DelProductFromChart(id, cookieValue);

            return RedirectToAction("Index", "ShoppingChart");
        }




    }
}
