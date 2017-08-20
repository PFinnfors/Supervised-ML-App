using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using BayesApp.Models;

namespace BayesApp.Controllers
{
    public class WeatherController : Controller
    {
        public async Task<string> GetWeather()
        {
            //deserialize JSON directly from a file
            WeatherModel model = JsonConvert.DeserializeObject<WeatherModel>(
                System.IO.File.ReadAllText(@"C:\DarkSkyAPI\key.json")
                );

            using (var client = new HttpClient())
            {
                
                HttpResponseMessage response = await client.GetAsync(
                    $"https://api.darksky.net/forecast/{model.key}/37.8267,-122.4233"
                    );

                if (response.IsSuccessStatusCode)
                {
                    string dataStr = await response.Content.ReadAsStringAsync();
                    JObject data = new JObject(dataStr);
                    

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