using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class CustomersController : Controller
    {
        private TheDatabase db = new TheDatabase();
        private ShoppingChart cart = ShoppingChart.getInstance();
        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,FName,LName,Adress,PostNr,City,Email,Phone,Comment")] Customer customer)
        {


            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();

                //ViewData["orderid"] = createOrder(customer);
                //return RedirectToAction("Index");
            }

            return View(customer);
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
                    System.Console.WriteLine("empty basket motherfucker");
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
            if (IsOrderPossible() & lines.Current.Count > 0)
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
            else {
                throw new InvalidOperationException();
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

        /* private int createOrder(Customer customer)
         {
             Order order = new Order();

             order.CustomerID = customer.CustomerID;

             db.Orders.Add(order);

             db.SaveChanges();

             DbSqlQuery<Order> justSavedOrder = db.Orders.SqlQuery("SELECT * FROM Orders WHERE CustomerID=@custid;", new SqlParameter("@custid", customer.CustomerID));

             return justSavedOrder.First().OrderID;

         }*/

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,FName,LName,Adress,PostNr,City,Email,Phone,Comment")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }




        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
