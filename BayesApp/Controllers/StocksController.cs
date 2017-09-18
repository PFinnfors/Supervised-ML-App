using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using BayesApp.Models;
using System;
using Accord.Statistics.Models.Regression.Linear;

namespace BayesApp.Controllers
{
    public class StocksController : Controller
    {
        public double[] inputs;
        public double[] outputs = { 80, 60, 40, 20, 0 };
        public OrdinaryLeastSquares ols;
        public SimpleLinearRegression regression;

        public StocksController()
        {
            this.ols = new OrdinaryLeastSquares();
            this.regression = ols.Learn(inputs, outputs);
        }

        public async Task<string> Get()
        {
            //deserialize JSON directly from a file
            StocksModel model = JsonConvert.DeserializeObject<StocksModel>
                (System.IO.File.ReadAllText(@"C:\AlphaVantageAPI\key.json"));
            
            //StockDate sD = new StockDate();

            //DateTime dt = new DateTime();
            //dt = DateTime.UtcNow;
            //dt = dt.AddDays(-13);

            //for (int i = 0; i < 0; i++)
            //{
            //    dt = dt.AddDays(-1);
            //}
            
            //Get Weather Date
            //sD = GetDate(dt, sD);

            using (var client = new HttpClient())
            {
                //
                HttpResponseMessage response = await client.GetAsync(
                    $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=MSFT&apikey={model.key}");

                if (response.IsSuccessStatusCode)
                {
                    string dataStr = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(dataStr);
                    model.TimeSeries = (JObject)data["Time Series (Daily)"];
                    


                }
                else
                {
                    var status = response.StatusCode;
                    var phrase = response.ReasonPhrase;
                    var msg = response.RequestMessage;
                }
            }

            return null;
        }

        public void ff()
        {

        }
            
    }
}