﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication3.Models
{
    public class ChartObject
    {
        public string ProdName { get; set; }

        public double Price { get; set; }

        public int Id { get; set; }

        public int Count { get; set; }

        public double ObjectTotal { get; set; }

    }

    public class ShoppingChart: IEnumerable<ChartObject>
    {
        private static ShoppingChart shoppingChart = new ShoppingChart();

        private TheDatabase db = new TheDatabase();
        private static List<ChartObject> theChart = new List<ChartObject>();
        private Product product;
        private ChartObject lastInserted;
        private double ShoppingChartTotalExclTax;
        private double ShoppingChartTotalInclTax;

        public double total { get; set; }


        private ShoppingChart()
        {
            lastInserted = new ChartObject();
            this.total = 0;
        }

        public static ShoppingChart getInstance()
        {
            return shoppingChart;

        }

        public void AddProductToChart(int? id, int? count)
        {

            if (id != null && count != null)
                 
            {
                if (!IsProductInChart(id)){
                    ChartObject chartObject = new ChartObject();

                product = db.Products.Find(id);

                chartObject.Id = (int)id;
                chartObject.ProdName = product.ArtName;
                chartObject.Price = product.Price;
                chartObject.Count = (int)count;
                chartObject.ObjectTotal = chartObject.Price * chartObject.Count;

                    
                copy(chartObject);

                theChart.Add(chartObject);

                this.total += chartObject.ObjectTotal;

                    }
                else
                {
                    foreach (var prod in theChart)
                    {
                        if (prod.Id == id)
                        {
                            prod.Count += (int)count;
                            prod.ObjectTotal += prod.Price * (int)count;
                            this.total += prod.Price * (int)count;

                            copy(prod);
                        }
                    }
                }
                }
        }

        public void  DelProductFromChart(int? id)
        {
            if (id != null)
            {

                try
                {
                    foreach (var prod in theChart)
                    {
                        if (prod.Id == id)
                        {
                            this.total -= prod.ObjectTotal;
                            theChart.Remove(prod);
                        }
                    }
                }
                catch (System.InvalidOperationException)
                {
                    ;
                }


            }

        }

        public bool IsProductInChart(int? id)
        {
            bool isInChart = false;

            if (id != null)
            {

                foreach (var prod in theChart)
                {
                    if (prod.Id == id)
                    {
                        isInChart = true;
                    }
                }
            }

            return isInChart;
        }


        public IEnumerator<ChartObject> GetEnumerator()
        {
            return theChart.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return theChart.GetEnumerator();
        }

        public ChartObject LastAddedProduct()
        {
            return lastInserted;
        }

        private void copy(ChartObject obj)
        {
            lastInserted.Id = obj.Id;
            lastInserted.ObjectTotal = obj.ObjectTotal;
            lastInserted.Price = obj.Price;
            lastInserted.ProdName = obj.ProdName;
            lastInserted.Count = obj.Count;
        }

        public double TotalSumExclTax()
        {
            ShoppingChartTotalInclTax = 0;
            ShoppingChartTotalExclTax = 0;

            foreach (var item in theChart)
            {
                ShoppingChartTotalExclTax += item.ObjectTotal;

            }

            ShoppingChartTotalInclTax = ShoppingChartTotalExclTax * 1.25;

            return ShoppingChartTotalExclTax;
        }

        public double TotalSumInclTax()
        {

            return ShoppingChartTotalExclTax * 1.25;
        }

    }
}
