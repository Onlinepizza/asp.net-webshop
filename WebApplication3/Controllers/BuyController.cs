using System;
using System.Collections;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder([Bind(Include = "CustomerID,FName,LName,Adress,PostNr,City,Email,Phone,Comment")] Customer customer)
        {
            if (ModelState.IsValid)
            {

                db.Customers.Add(customer);
                db.SaveChanges();
                try
                {
                    Buy(customer.CustomerID);
                }
                catch (InvalidOperationException e)
                {

                    //delete last customer
                }
                //ViewData["orderid"] = createOrder(customer);
                //return RedirectToAction("Index");
            }

            return View(customer);
        }

        public void Buy(int id)
        {
            var lines = cart.GetEnumerator();
            if (IsOrderPossible())
            {
                Order newOrder = new Order();
                db.Orders.Add(newOrder);
                db.SaveChanges();
                int identity = db.Orders.Last().OrderID;
                db.Orders.Last().CustomerID = id;
                db.SaveChanges();

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
            else { throw new InvalidOperationException();
            }
            
            //exception
        }

        public bool IsOrderPossible()
        {
            bool i = true;
            var Lines = cart.GetEnumerator();
            while (i)
            {
                var prod = db.Products.Find(Lines.Current.Id);
                if (prod.InStock < Lines.Current.Count)
                {
                    return false;
                }
                else {
                    i = Lines.MoveNext();
                }
            }
            Lines.Reset();
            return true;
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
