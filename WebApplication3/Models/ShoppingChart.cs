using System.Collections;
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

        public double total { get; set; }


        private ShoppingChart()
        {
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
                        }
                    }
                }
                }
        }

        public void  DelProductFromChart(int? id)
        {
            if (id != null)
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

   }
}
