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
    public class OrderlinesController : Controller
    {
        private TheDatabase db = new TheDatabase();

        // GET: Orderlines
        public ActionResult Index()
        {
            return View(db.Orderlines.ToList());
        }

        // GET: Orderlines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderline orderline = db.Orderlines.Find(id);
            if (orderline == null)
            {
                return HttpNotFound();
            }
            return View(orderline);
        }

        // GET: Orderlines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orderlines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderlineID,ArtID,OrderID,Antal")] Orderline orderline)
        {
            if (ModelState.IsValid)
            {
                db.Orderlines.Add(orderline);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderline);
        }

        // GET: Orderlines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderline orderline = db.Orderlines.Find(id);
            if (orderline == null)
            {
                return HttpNotFound();
            }
            return View(orderline);
        }

        // POST: Orderlines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderlineID,ArtID,OrderID,Antal")] Orderline orderline)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderline).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderline);
        }

        // GET: Orderlines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderline orderline = db.Orderlines.Find(id);
            if (orderline == null)
            {
                return HttpNotFound();
            }
            return View(orderline);
        }

        // POST: Orderlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orderline orderline = db.Orderlines.Find(id);
            db.Orderlines.Remove(orderline);
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
    }
}
