using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace WebApplication3.Models
{
    public class ChartObject
    {
        public string ProdName { get; set; }

        public double Price { get; set; }

        public double costPrice { get; set; }

        public int Id { get; set; }

        public int Count { get; set; }

        public double ObjectTotal { get; set; }

    }

    internal class Message
    {
        public string OrderMessage { get; set; }
    }


    internal class ChartComplementary
    {
        public ChartObject lastInserted { get; set; }
        public  double ShoppingChartTotalExclTax { get; set; }
        public double ShoppingChartTotalInclTax { get; set; }

        public Product prodRow;

        public Message message;

        public ChartComplementary()
        {
            lastInserted = new ChartObject();
            prodRow = new Product();

            message = new Message();
        }
    }

    public class ShoppingChart
    {
        private static ShoppingChart shoppingChart = new ShoppingChart();
        
        private static TheDatabase db = new TheDatabase();
        private static Dictionary<string, Dictionary<int, ChartObject>> allCarts;
        private static Dictionary<string, ChartComplementary> complementaryCarts;

        private ShoppingChart()
        {
            allCarts = new Dictionary<string, Dictionary<int, ChartObject>>();
            complementaryCarts = new Dictionary<string, ChartComplementary>();
        }

        public static ShoppingChart getInstance()
        {
            return shoppingChart;

        }

        public static void InitializeShoppingChart(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (!allCarts.ContainsKey(cartName))
            {
                allCarts.Add(cartName, new Dictionary<int, ChartObject>());
                complementaryCarts.Add(cartName, new ChartComplementary());
            }
        }

        public static void AddProductToChart(int id, int count, string encodedCookieValue)
        {

            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (!allCarts[cartName].ContainsKey(id))
            {
                allCarts[cartName].Add(id, new ChartObject());

                saveNewProductInCart(cartName, id, count);
            }
            else
                updateProductInCart(cartName, id, count);

        }


        private static void updateProductInCart(string cartName, int id, int count)
        {
            ChartObject chartObject = allCarts[cartName][id];

            chartObject.Count += count;
            chartObject.ObjectTotal += chartObject.Price * count;

            saveLastProduct(chartObject, cartName);
        }

        private static void saveNewProductInCart(string cartName, int id, int count)
        {
            Product product = db.Products.Find(id);
            ChartObject chartObject = allCarts[cartName][id];


            if (product != null)
            {
                chartObject.Id = (int)id;
                chartObject.ProdName = product.ArtName;
                chartObject.Price = product.Price;
                chartObject.Count = (int)count;
                chartObject.ObjectTotal = chartObject.Price * chartObject.Count;

                saveLastProduct(chartObject, cartName);
            }
        }

        public static void DelProductFromChart(int id, string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

                if (allCarts.ContainsKey(cartName))
                    if (allCarts[cartName].ContainsKey(id))
                        allCarts[cartName].Remove(id);
        }


        public static IEnumerator<ChartObject> GetEnumerator(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (allCarts.ContainsKey(cartName))
                return allCarts[cartName].Values.OfType<ChartObject>().GetEnumerator();

            return null;
        }

        /*
        public IEnumerator<ChartObject> GetEnumerator()
        {
            string cartName = CookieModel.GetCartName("2");

            if (allCarts.ContainsKey(cartName))
                return allCarts[cartName].Values.OfType<ChartObject>().GetEnumerator();

            return null;
        }

    */
        public static ChartObject LastAddedProduct(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (allCarts.ContainsKey(cartName))
                return complementaryCarts[cartName].lastInserted;

            return new ChartObject();
        }

        private static void saveLastProduct(ChartObject obj, string cartName)
        {
            complementaryCarts[cartName].lastInserted = obj;
        }

        public static double TotalSumExclTax(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (complementaryCarts.ContainsKey(cartName))
            {
                ChartComplementary cartCompl = complementaryCarts[cartName];

                cartCompl.ShoppingChartTotalInclTax = 0;
                cartCompl.ShoppingChartTotalExclTax = 0;



                foreach (ChartObject item in allCarts[cartName].Values)
                    cartCompl.ShoppingChartTotalExclTax += item.ObjectTotal;

                return cartCompl.ShoppingChartTotalExclTax;
            }

            return 0;
        }

        public static double TotalSumInclTax(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (complementaryCarts.ContainsKey(cartName))
                return complementaryCarts[cartName].ShoppingChartTotalExclTax * 1.25;

            return 0;
        }

        public static bool CheckoutProducts(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            switch (allCarts.ContainsKey(cartName))
            {
                case true:
                    complementaryCarts[cartName].message.OrderMessage = "";

                    DbContextTransaction myTrans;
                        myTrans = db.Database.BeginTransaction();

                        try
                        {
                            //db.Database.ExecuteSqlCommand(
                            //    "INSERT INTO Products (ArtName, InStock, Price, Descr) VALUES('Prototype', 7, 88, @descr); SELECT * FROM Dejligt;"
                            //    , new SqlParameter("@descr", "An even newer product"));
                            foreach (ChartObject item in allCarts[cartName].Values)
                            {
                                complementaryCarts[cartName].prodRow = db.Products.Find(item.Id);

                                if (complementaryCarts[cartName].prodRow != null)
                                {
                                    db.Database.ExecuteSqlCommand(
                                    "UPDATE Products SET InStock=@num WHERE ProductID=@ID;"
                                    , new SqlParameter("@num", complementaryCarts[cartName].prodRow.InStock - item.Count), new SqlParameter("@ID", item.Id));
                                }
                            }

                            myTrans.Commit();
                        }
                        catch (System.Exception e)
                        {
                            complementaryCarts[cartName].message.OrderMessage = "Could not finalize order. Not enough " + complementaryCarts[cartName].prodRow.ArtName + " in store.";
                            myTrans.Rollback();
                        }
                        finally
                        {
                            myTrans.Dispose();
                        }

                        if (complementaryCarts[cartName].message.OrderMessage != "")
                            return false;

                        
                        complementaryCarts[cartName].message.OrderMessage = "Finished Checkout successfully";
                        return true;

                case false:
                    complementaryCarts[cartName].message.OrderMessage = "No Shoppingcart found. The ShoppingCart cookie was probably changed out of our control.";
                    return false;

                default:
                    complementaryCarts[cartName].message.OrderMessage = "Unknown error";
                    return false;
            }
        }

        public static void emptyChart(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (allCarts.ContainsKey(cartName))
                allCarts[cartName].Clear();
        }

        public static List<ChartObject> GetChartObjects(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if(allCarts.ContainsKey(cartName))
                return allCarts[cartName].Values.ToList<ChartObject>();

            return null;
        }

        public static string GetOrderMessage(string encodedCookieValue)
        {
            string cartName = CookieModel.GetCartName(encodedCookieValue);

            if (complementaryCarts.ContainsKey(cartName))
                return complementaryCarts[cartName].message.OrderMessage;

            return "Internal error: OrderMessage unreachable for that ShoppingCart.";
        }
    }
}

