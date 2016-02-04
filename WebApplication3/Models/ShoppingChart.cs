using System.Collections.Generic;
using System.Linq;

namespace WebApplication3.Models
{
    public class ShoppingChart
    {
        private ShoppingChart shoppingChart;

        private TheDatabase db = new TheDatabase();
        private List<ChartObject> theChart;
        private Product product;


        private ShoppingChart()
        {
            ShoppingChart shoppingChart = new ShoppingChart();

            shoppingChart.theChart = new List<ChartObject>();
        }

        public ShoppingChart getInstance()
        {
            return shoppingChart;

        }

        public void AddProductToChart(int? id, int count)
        {

            if (id != null)
            {
                ChartObject chartObject = new ChartObject();

                product = db.Products.Find(id);

                chartObject.Id = (int)id;
                chartObject.ProdName = product.ArtName;
                chartObject.Price = product.Price;
                chartObject.Count = count;
                theChart.Add(chartObject);
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

        public string getChartObjectProdName(int index)
        {
            return theChart.ElementAt(index).ProdName;
        }

        public double getChartObjectPrice(int index)
        {
            return theChart.ElementAt(index).Price;
        }

        public int getChartObjectId(int index)
        {
            return theChart.ElementAt(index).Id;
        }

        public int getChartObjectCount(int index)
        {
            return theChart.ElementAt(index).Count;
        }

        public class ChartObject
        {
            public string ProdName { get; set; }

            public double Price { get; set; }

            public int Id { get; set; }

            public int Count { get; set; }
            
        }

   }
}
