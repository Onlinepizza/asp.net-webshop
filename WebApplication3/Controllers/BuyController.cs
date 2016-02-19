using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class BuyController : Controller
    {
        private TheDatabase db = new TheDatabase();
        private ShoppingChart cart = ShoppingChart.getInstance();
        // GET: Buy
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOrder()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder([Bind(Include = "CustomerID,FName,LName,Adress,PostNr,City,Email,Phone,Comment")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                
                db.Customers.Add(customer);
                db.SaveChanges();
                Buy(customer.CustomerID);
                //ViewData["orderid"] = createOrder(customer);
                //return RedirectToAction("Index");
            }

            return View(customer);
        }

        public void Buy(int id)
        {
            Order newOrder = new Order();
            db.Orders.Add(newOrder);
            db.SaveChanges();
            int identity = db.Orders.Last().OrderID;
            db.Orders.Last().CustomerID = id;
            db.SaveChanges();
            var lines = cart.GetEnumerator();
            int e = 1;
            bool breakBool = true;
            while (e != 0)
            {
                Orderline Oline = new Orderline();
                Oline.Antal = lines.Current.Count;
                Oline.ArtID = lines.Current.Id;
                Oline.OrderID = identity;
                db.Orderlines.Add(Oline);
                db.SaveChanges();
                breakBool = lines.MoveNext();
                if (!breakBool)
                {
                    e = 0;
                }
            }
        }

        // GET: Buy/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Buy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Buy/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Buy/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Buy/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Buy/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Buy/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
