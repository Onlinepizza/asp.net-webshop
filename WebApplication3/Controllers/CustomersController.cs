﻿using System;
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
                customer = formatUserInput(customer);
                db.Customers.Add(customer);
                db.SaveChanges();

                //ViewData["orderid"] = createOrder(customer);
                //return RedirectToAction("Index");
            }

            return View(customer);
        }

        //Format the user input strings when creating or editing user
        public Customer formatUserInput(Customer customer)
        {

            customer.FName = formatString(customer.FName);
            customer.LName = formatString(customer.LName);
            customer.Adress = formatString(customer.Adress);
            customer.City = formatString(customer.City);
            customer.Email = customer.Email.ToLower();
            return customer;
        }

        //format the current string
        public string formatString(string theString)
        {
          
            string head;
            string tail;

            head = theString[0].ToString();
            head = head.ToUpper();

            tail = theString.Substring(1);
            tail = tail.ToLower();


            return string.Concat(head, tail);
        }

        public ActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder([Bind(Include = "CustomerID,FName,LName,Adress,PostNr,City,Email,Phone,Comment")] Customer customer)
        {
            string cookieValue;

                if (CookieModel.IsCookieValid(Request, out cookieValue) && ModelState.IsValid)
                {
                customer = formatUserInput(customer);
                db.Customers.Add(customer);
                    db.SaveChanges();
                    try
                    {
                        Buy(customer.CustomerID, cookieValue);
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


        private void createNewOrder(int customerId,  out int orderId)
        {
            Order newOrder = new Order();
            db.Orders.Add(newOrder);
            db.SaveChanges();

            orderId = db.Orders.OrderByDescending(x => x.OrderID).Take(1).Single().OrderID;

            db.Orders.OrderByDescending(x => x.OrderID).Take(1).Single().CustomerID = customerId;

            db.SaveChanges();

        }

        private void SaveOrderLines(int orderId, string encodedCookieValue)
        {

            foreach (ChartObject chartObj in ShoppingChart.GetChartObjects(encodedCookieValue))
            {
                Orderline Oline = new Orderline();
                Oline.Antal = chartObj.Count;
                Oline.ArtID = chartObj.Id;
                Oline.OrderID = orderId;
                db.Orderlines.Add(Oline);
                db.SaveChanges();
            }
        }

        public void Buy(int customerId, string encodedCookieValue)
        {
            int orderId = 0;

            if (ShoppingChart.CheckoutProducts(encodedCookieValue))
            {
                createNewOrder(customerId, out orderId);
                SaveOrderLines(orderId, encodedCookieValue);
                ShoppingChart.emptyChart(encodedCookieValue);
            }
            else
                throw new InvalidOperationException();

        }

        //public bool IsOrderPossible(string encodedCookieValue)
        //{
        //    Product prod;

        //    foreach (ChartObject chartObj in ShoppingChart.GetChartObjects(encodedCookieValue))
        //    {
        //       prod = db.Products.Find(chartObj.Id);

        //        if (prod == null)
        //            return false;

        //        if (prod.InStock < chartObj.Count)
        //            return false;
        //    }

        //    return true;
        //}

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
