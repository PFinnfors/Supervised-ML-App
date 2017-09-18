using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BayesApp.Models
{
    public class StocksModel
    {
        //stores AlphaVantage API key read from file
        public string key { get; set; }

        //stores AlphaVantage API data
        public JObject TimeSeries { get; set; }
    }
}