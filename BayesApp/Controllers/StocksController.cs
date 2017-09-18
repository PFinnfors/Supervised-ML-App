using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BayesApp.Models;
using Accord.Statistics.Models.Regression.Linear;

namespace BayesApp.Controllers
{
    public class StocksController : Controller
    {

        public async Task<string> Get()
        {
            //deserialize JSON directly from a file
            StocksModel model = JsonConvert.DeserializeObject<StocksModel>
                (System.IO.File.ReadAllText(@"C:\AlphaVantageAPI\key.json"));


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

                    double[] ins = new double[8];
                    ins[0] = (double)model.TimeSeries["2017-09-05"]["4. close"];
                    ins[1] = (double)model.TimeSeries["2017-09-06"]["4. close"];
                    ins[2] = (double)model.TimeSeries["2017-09-07"]["4. close"];
                    ins[3] = (double)model.TimeSeries["2017-09-08"]["4. close"];
                    ins[4] = (double)model.TimeSeries["2017-09-11"]["4. close"];
                    ins[5] = (double)model.TimeSeries["2017-09-12"]["4. close"];
                    ins[6] = (double)model.TimeSeries["2017-09-13"]["4. close"];
                    ins[7] = (double)model.TimeSeries["2017-09-14"]["4. close"];

                    double[] outs = new double[8];
                    outs[0] = (double)model.TimeSeries["2017-09-06"]["4. close"];
                    outs[1] = (double)model.TimeSeries["2017-09-07"]["4. close"];
                    outs[2] = (double)model.TimeSeries["2017-09-08"]["4. close"];
                    outs[3] = (double)model.TimeSeries["2017-09-11"]["4. close"];
                    outs[4] = (double)model.TimeSeries["2017-09-12"]["4. close"];
                    outs[5] = (double)model.TimeSeries["2017-09-13"]["4. close"];
                    outs[6] = (double)model.TimeSeries["2017-09-14"]["4. close"];
                    outs[7] = (double)model.TimeSeries["2017-09-15"]["4. close"];
                    
                    //Train Regression Model
                    MyRegressionModel myRM = new MyRegressionModel(ins, outs);
                    //Use last data to predict Monday close
                    var result = myRM.Predict((double)model.TimeSeries["2017-09-15"]["4. close"]).ToString();
                    return result;
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
            
    }
}