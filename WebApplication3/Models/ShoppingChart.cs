using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class ShoppingChart
    {
        private TheDatabase db = new TheDatabase();
        private List<ChartObject> theChart;
        private ChartObject chartObject;
        private Product product;

        public void AddProductToChart(int? id)
        {

            if (id != null)
            {
                product = db.Products.Find(id);

                chartObject.Id = (int)id;
                chartObject.ProdName =  product.ArtName;
                chartObject.Price = product.Price;

                theChart.Add(chartObject);
            }
            
        }

        public void  DelProductFromChart(int? id)
        {
            if (id != null)
            {

                //theChart.Find(System.Predicate < int> product);
            }

        }

        public bool IsProductInChart(int? id)
        {
            if (id != null)
            {

                //theChart.Find(System.Predicate < int> product);
            }

            return true;

        }

        private class ChartObject
        {
            public string ProdName { get; set; }

            public double Price { get; set; }

            public int Id { get; set; }
        }

   }
}