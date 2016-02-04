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
        }

        ShoppingChart getInstance()
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

        List<ChartObject> getChartObjects()
        {
            return theChart;
        }

        public string getProdName(ChartObject cobject){
            return cobject.ProdName;
        }

        public double getPrice(ChartObject cobject)
        {
            return cobject.Price;
        }

        public int getId(ChartObject cobject)
        {
            return cobject.Id;
        }

        public int getCount(ChartObject cobject)
        {
            return cobject.Count;
        }

        private class ChartObject
        {
            public string ProdName { get; set; }

            public double Price { get; set; }

            public int Id { get; set; }

            public int Count { get; set; }
            
        }

   }
}
